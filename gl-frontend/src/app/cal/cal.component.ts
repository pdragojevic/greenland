import { AfterViewInit, Component, Input, OnInit, ViewChild } from '@angular/core';
import { FullCalendarComponent, CalendarOptions, DateSelectArg, EventClickArg, EventApi, getEventClassNames } from '@fullcalendar/angular';
import { CalendarService } from '../_srvc/calendar.service';
import { createEventId } from './event-utils';
import {NgbModal, NgbTimeStruct,NgbTimepicker, NgbActiveModal, NgbDatepicker} from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import {MeetingModalComponent} from './meeting-modal/meeting-modal.component'
import { first } from 'rxjs/operators';
import { Employee } from '../_models/Employee';
import { TeamService } from '../_srvc/team.service';
import { EmployeeService } from '../_srvc/employee.service';

@Component({
  selector: 'app-calendar',
  templateUrl: './cal.component.html',
  styleUrls: ['./cal.component.scss']
})
export class CalComponent implements AfterViewInit {
  x: any;
  //-------------calendar options
  @ViewChild('calendar') calendarComponent: FullCalendarComponent;
  calendarVisible = true;
  calendarOptions: CalendarOptions = {
    headerToolbar: {
      left: 'prev,next today',
      center: 'title',
      right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
    },
    initialView: 'dayGridMonth',
    googleCalendarApiKey: 'AIzaSyDF8ezWNLWvS87QKgXvveLQJ8L5cFd54IQ',
    weekends: true,
    editable: true,
    selectable: true,
    selectMirror: true,
    dayMaxEvents: true,
    select: this.handleDateSelect.bind(this),
    eventClick: this.handleEventClick.bind(this),
    eventsSet: this.handleEvents.bind(this)
    /* you can update a remote database when these fire:
    eventAdd:
    eventChange:
    eventRemove:
    */
  };
  currentEvents: EventApi[] = [];
  //meeting vars
  team: any;
  emp: any;
  canCreateMeeting: boolean;

  constructor(
    private calendarService: CalendarService,
    private ngbModal: NgbModal,
    private formBuilder: FormBuilder,
    private teamService: TeamService,
    private employeeService: EmployeeService
  ) {
    this.calendarService.getEvents().subscribe(x=>{
      this.calendarOptions.events = x
      this.x=JSON.stringify(x)
    });
    this.teamService.getActivityFeedUsers().pipe(first()).subscribe(team=>{
      this.team=team.map(teammember => {return {email: teammember.email, displayName: teammember.firstName+" "+teammember.lastName}}
      )
      console.log(this.team);

      this.employeeService.getEmployeeLogged().pipe(first()).subscribe(emp => {
        if (emp.position=="Administration" || emp.position=="Team leader" || emp.position=="Coordinator") {
          this.canCreateMeeting=true;
        }
      });
    });
  }

  ngOnInit() {
  }
  
  ngAfterViewInit() {
    //this.calendarService.getEvents().subscribe(x=>this.x=JSON.stringify(x));
    //let calendarApi = this.calendarComponent.getApi();
    //calendarApi.setOption("eventSources",[{googleCalendarId: 'ldosen@vrkic.co'}])
    
  }

  handleCalendarToggle() {
    //this.calendarVisible = !this.calendarVisible;
    //this.calendarService.getEvents().subscribe(x=>this.x=JSON.stringify(x));
    let calendarApi = this.calendarComponent.getApi();
    calendarApi.addEvent(this.x)
  }

  handleWeekendsToggle() {
    const { calendarOptions } = this;
    calendarOptions.weekends = !calendarOptions.weekends;
  }

  handleDateSelect(selectInfo: DateSelectArg) {
    if (this.canCreateMeeting==true) {
      const calendarApi = selectInfo.view.calendar;
      calendarApi.unselect(); // clear date selection
      var s =selectInfo.start
      const modalRef = this.ngbModal.open(MeetingModalComponent);
      modalRef.componentInstance.delMode=false;
      modalRef.componentInstance.start = {year: s.getFullYear(), month: s.getMonth()+1, day: s.getDate()}
      modalRef.componentInstance.meetingTime = {hour: s.getHours(), minute: s.getMinutes(), second: 0}
      let id = createEventId()
      modalRef.componentInstance.title='Sastanak '+id
      modalRef.componentInstance.duration='30'
      modalRef.result.then((result) => {
        if (result) {
        this.addgCalMeeting(result)
        result.id=id
        calendarApi.addEvent(result);
        }
        });
    } else {
        alert("You don't have privilege to create new meetings.")
    }
  }

  handleEventClick(clickInfo: EventClickArg) {
    this.x=JSON.stringify(clickInfo.event)
    var s= clickInfo.event.start
    const diffTime = Math.abs(<any>clickInfo.event.end - <any>clickInfo.event.start);
    const diffMins = Math.ceil(diffTime / (1000 * 60))

    const modalRef = this.ngbModal.open(MeetingModalComponent);
    modalRef.componentInstance.delMode=true;
    modalRef.componentInstance.delModeDate=s.toString()
    modalRef.componentInstance.start = {year: s.getFullYear(), month: s.getMonth()+1, day: s.getDate()}
    modalRef.componentInstance.meetingTime = {hour: s.getHours(), minute: s.getMinutes(), second: 0}
    let id = createEventId()
    modalRef.componentInstance.title=clickInfo.event.title
    modalRef.componentInstance.duration=diffMins
    if (clickInfo.event.extendedProps.description){
      modalRef.componentInstance.description=clickInfo.event.extendedProps.description
    }
    if (clickInfo.event.extendedProps.location){
      modalRef.componentInstance.location=clickInfo.event.extendedProps.location
    }
    modalRef.result.then((result) => {
      if (result) {
      // this.addgCalMeeting(result)
      // result.id=id
      
      }

      });
    // if (confirm(`Are you sure you want to delete the event '${clickInfo.event.title}'`)) {
    //   clickInfo.event.remove();
    // }
    //this.ngbModal.open(content, {ariaLabelledBy: 'modal-basic-title'})
  }

  handleEvents(events: EventApi[]) {
    this.currentEvents = events;
  }


   updateEvents() {
    const nowDate = new Date();
    const yearMonth = nowDate.getUTCFullYear() + '-' + (nowDate.getUTCMonth() + 1);
    console.log(JSON.parse(this.x))
    
  }

  open() {
    if (this.canCreateMeeting==true) {
      let calendarApi = this.calendarComponent.getApi();
      const modalRef = this.ngbModal.open(MeetingModalComponent);
      modalRef.componentInstance.delMode=false;
      let id = createEventId()
      modalRef.componentInstance.title='Sastanak '+id
      modalRef.componentInstance.duration='30'
      modalRef.result.then((result) => {
        if (result) {
        this.addgCalMeeting(result)
        result.id=id
        calendarApi.addEvent(result);
        console.log("adding event")
        
        }

        });
    } else {
      alert("You don't have privilege to create new meetings.")
    }
  }
 
  addgCalMeeting(event) {
    event.attendees=this.team
    this.calendarService.insertEvent(event)
        .pipe(first())
        .subscribe({
            next: () => {
              alert("event added")
                
            },  
            error: err => {
              if (err.status == 500)
              alert(err.status)

            }
        });
  }
};
