import { Component, Input, OnInit, Output } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { NgbActiveModal, NgbTimeStruct, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { GCalEvent } from 'src/app/_models/GCalEvent';

@Component({
  selector: 'app-meeting-modal',
  templateUrl: './meeting-modal.component.html'
})
export class MeetingModalComponent implements OnInit {
  @Input() delMode: boolean
  @Input() delModeDate: any
  @Input() meetingTime: NgbTimeStruct //= {hour: 12, minute: 0, second: 0};
  @Input() start: NgbDateStruct;
  @Input() duration: String;
  @Input() title: String;
  @Input() description: String;
  @Input() location: String;
  

  hourStep = 1;
  minuteStep = 15;
  secondStep = 30;
  ctrl = new FormControl('', (control: FormControl) => {return null});
  constructor(public modal: NgbActiveModal) {
   
   }

  ngOnInit(): void {
  }
  addMeeting() {
    console.log(this.start)
    console.log(this.title)
  }

  delete(date1,date2) {
    const diffTime = Math.abs(date2 - date1);
    const diffDays = Math.ceil(diffTime / (1000 * 60))
  }
   

  passBack() {
    var sD= new Date(this.start.year, this.start.month - 1, this.start.day, this.meetingTime.hour, this.meetingTime.minute);
    var eD = new Date(sD)
    eD.setMinutes(eD.getMinutes() + Number(this.duration))
    this.modal.close({
      title: this.title,
      description: this.description,
      start: sD,
      end: eD,
      location: this.location
      });
  }
}
