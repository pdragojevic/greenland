import { Component, Input, OnInit} from '@angular/core';
import { FormControl } from '@angular/forms';
import { NgbActiveModal, NgbTimeStruct, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { TaskService } from 'src/app/_srvc/task.service';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-task-modal',
  templateUrl: './task-modal.component.html',
  styleUrls: ['./task-modal.component.css']
})
export class TaskModalComponent implements OnInit {
  @Input() creationDate: NgbDateStruct;
  @Input() deadline: NgbDateStruct;
  @Input() description: String;
  @Input() idTask: String;
  @Input() priority: String;
  @Input() status: String;
  @Input() summary: String;
  @Input() team: any;
  @Input() assignedId: any;
  test: any
  ctrl = new FormControl('', (control: FormControl) => {return null});
  taskService: TaskService
  constructor(public modal: NgbActiveModal,) {
   
   }

  ngOnInit(): void {

  }
  addMeeting() {
  }


  handleChange(index) {
    this.assignedId=this.team[index].idEmployee;
  }
  passBack() {
    var sD= new Date(this.creationDate.year, this.creationDate.month - 1, this.creationDate.day);
    sD = new Date(sD.getTime() - sD.getTimezoneOffset() * 60000)
    
    var eD = new Date(this.deadline.year, this.deadline.month - 1, this.deadline.day)
    eD = new Date(eD.getTime() - eD.getTimezoneOffset() * 60000)
    this.modal.close([{
      idTask: Number(this.idTask),
      summary: this.summary,
      description: this.description,
      creationDate: sD,
      deadline: eD,
      priority: Number(this.priority),
      status: this.status
      }, {idTask: Number(this.idTask),
      idEmployee: Number(this.assignedId)
      }]);
  }

  delete() {
    this.modal.close([
      {idTask: Number(this.idTask)
      }]);
  }
}
