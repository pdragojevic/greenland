import { Component, OnInit } from '@angular/core';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { Column } from '../_models/column.model';
import { Board } from '../_models/board.model';
import { Task } from '../_models/Task';
import { TaskService } from '../_srvc/task.service';
import { first } from 'rxjs/operators';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { templateJitUrl } from '@angular/compiler';
import { EmployeeService } from '../_srvc/employee.service';
import { EmployeeLogged } from '../_models/EmployeeLogged';
import { DatePipe } from '@angular/common';
import { Team } from '../_models/Team';
import { TeamService } from '../_srvc/team.service';
import { Iteration } from '../_models/Iterations';
import { parse } from 'path';
import { TaskModalComponent } from './task-modal/task-modal.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {
  tasks: Task[];
  tasksNew: Task[] = [];
  tasksIP: Task[] = [];
  tasksTesting: Task[] = [];
  tasksResolved: Task[] = [];
  board: Board;
  isDisplay = true;
  isOwner = false;
  teamSelected = false;
  editTaskDisplay = false;
  showIterations = false;
  newTask: Task;
  emp: EmployeeLogged;
  team: Team;
  teamMembers: any;
  teamList: any;
  toggledTeamId: Number;
  pipe = new DatePipe('en_US');
  currentIteration: Iteration;
  iterations: Iteration[];
  startDate : string;
  endDate : String;
  activeIteration : number;

  columnIdentificator : String;
  cdkDropNumber : String;

  addTaskForm: FormGroup;
  editTaskForm: FormGroup;
  
  constructor(private taskService: TaskService, private fb: FormBuilder, private employeeService: EmployeeService, private teamService: TeamService, private ngbModal: NgbModal) { }

  toggleOwner(){
    this.isOwner = !this.isOwner;
  }
  toggleShowIterations() {
    this.showIterations = !this.showIterations
  }
  toggleDisplay() {
    this.isDisplay = !this.isDisplay;
  }
  editTaskDisplayMethod() {
    this.editTaskDisplay = !this.editTaskDisplay
  }

  ngOnInit(): void {
    if (sessionStorage.getItem('activeIteration') === null) {
        sessionStorage.setItem('activeIteration', "6");
        this.activeIteration=6;
    } else {
      this.activeIteration = sessionStorage.getItem('activeIteration') as unknown as number;
    }
    
    this.teamService.getActivityFeedUsers().pipe(first()).subscribe(team=>{
      this.teamMembers=team
    });

    this.employeeService.getEmployeeLogged().pipe(first()).subscribe(emp => {
      this.emp = emp;
      this.emp.hireDate = this.pipe.transform(this.emp.hireDate, 'longDate');
      this.emp.birthDate = this.pipe.transform(this.emp.birthDate, 'longDate');

      if (this.emp.position == 'Administration') {
        this.toggleOwner();
        this.teamService.getAllTeams().pipe(first()).subscribe(teams => {
          this.teamList = teams;
          this.changeIteration(this.activeIteration, this.teamList[0].idTeam)
          this.toggledTeamId=this.teamList[0].idTeam;
        })
      } else if (this.emp.position == 'Coordinator') {
        this.toggleOwner();
        this.teamService.getAllTeamsFromCoordinator(emp.idEmployee).pipe(first()).subscribe(teams => {
          console.log(teams);
          this.teamList = teams;
          this.changeIteration(this.activeIteration, this.teamList[0].idTeam)
          this.toggledTeamId=this.teamList[0].idTeam;
        })
      } else {
        this.changeIteration(this.activeIteration);
      }
    });



    if (localStorage.getItem('currentIteration') === null) {
      console.log("Nema iteracije");
      this.taskService.getFirstIteration().pipe(first()).subscribe(iter => {
        localStorage.setItem('currentIteration', JSON.stringify(iter));
        document.location.reload(true);
      })
    }

    this.currentIteration =JSON.parse(localStorage.getItem('currentIteration'));

   //problem je ako mora poslat zahtjev za currentIteration i ako mora uć u ovaj if ispod jer je currentIteration onda == NULL
   //rjesenje je koristit promise
   //ko ga jebe stavio sam reload
   var myDate = new Date();
   //console.log("My date: " + myDate);
   if(localStorage.getItem('iterations') === null || myDate > this.currentIteration.end) {
     //console.log("Moram slat zahtjev!");
     if(this.currentIteration === null) {
       //console.log("NULL");
     }
     this.taskService.getIterations(this.currentIteration).pipe(first()).subscribe(iterations => {
       localStorage.setItem('iterations', JSON.stringify(iterations));
      // console.log("Novi current iteration: " + JSON.stringify((iterations as unknown as Array<Iteration>)[5]));
       localStorage.setItem('currentIteration', JSON.stringify((iterations as unknown as Array<Iteration>)[5]));
     });
   }
   this.iterations = JSON.parse(localStorage.getItem('iterations'));        //ovako radi!

    //adding task variables
    this.addTaskForm = this.fb.group({
      t_summary: ['', Validators.required],
      t_desc: ['', Validators.required],
      t_priority: ['', Validators.required],
      t_deadline: ['', Validators.required],
      t_status: ['', Validators.required],

    });

    this.editTaskForm = this.fb.group({
      t_status: ['', Validators.required]
    });

    //this.start
    //this.end

    console.log("predani argument start je: ", JSON.stringify(this.iterations[5].start).substring(1,11))
    console.log("predani argument start je: ", JSON.stringify(this.iterations[5].end).substring(1,11))
    //this.activeIteration = 5
    this.startDate = JSON.stringify(this.iterations[this.activeIteration].start).substring(1,11)
    this.endDate = JSON.stringify(this.iterations[this.activeIteration].end).substring(1,11)

    //this.changeIteration(5);
  
    /*this.taskService.getTaskBetween(-1, this.startDate, this.endDate).pipe(first()).subscribe(tasks=>{
      this.tasks=tasks;
      console.log(this.tasks)

      //selecting column for task based on status
      //moraju se ispraznit stupci prije nego se opet napune s dohvacenim taskovima(inace se taskovi krenu duplirat svaki put)
      this.tasksNew = []
      this.tasksIP = []
      this.tasksTesting = []
      this.tasksResolved = []
      for (let i of this.tasks) {
        if ((i.status) == 'new') {
          this.tasksNew.push(i);
        }
        else if ((i.status) == 'in progress') {
          this.tasksIP.push(i);
        }
        else if ((i.status) == 'testing column') {
          this.tasksTesting.push(i);
        }
        else {
          this.tasksResolved.push(i);

        }
      }



      this.board = new Board('Test Board', [        
        new Column('New', this.tasksNew),
        new Column('In progress', this.tasksIP),
        new Column('Testing Column', this.tasksTesting),
        new Column('Resolved', this.tasksResolved)
      ]) 
    }); */
  }


  drop(event: CdkDragDrop<Task[]>, column: Column) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    }
    else {
      console.log("prije")
      console.log(event.container.data.length)
      if (event.container.data.length > 9) {
        alert("The selected column already has 10 tasks")
      }
      else {
        transferArrayItem(event.previousContainer.data,
          event.container.data,
          event.previousIndex,
          event.currentIndex);
          //console.log('prije mjenjanja statusa')
          this.columnIdentificator = JSON.stringify(event.container.id)
          var numb = this.columnIdentificator.replace( /^\D+/g, '')
          //console.log(numb.slice(0,-1))
          var broj = parseInt(numb.slice(0, -1))
          //console.log(broj)
          

          if(broj % 4 == 0){
            console.log("treba biti new")
            event.container.data[event.currentIndex].status = 'new'
          }
          if(broj % 4 == 1){
            console.log("treba biti inprogress")
            event.container.data[event.currentIndex].status = 'in progress'
          }
          if(broj % 4 == 2){
            console.log("treba biti TC")
            event.container.data[event.currentIndex].status = 'testing column'
          }
          if(broj % 4 == 3){
            console.log("treba biti resolved")
            event.container.data[event.currentIndex].status = 'resolved'
          }
          //console.log(event.container.data[event.currentIndex].idTask)
          //console.log(event.container.data[event.currentIndex].status)

          this.taskService.updateTaskStatus(event.container.data[event.currentIndex].idTask, event.container.data[event.currentIndex].status).pipe(first()).subscribe(task => {

        });
      }
    }
  }

  done() {
    //dodaj refresh na kraju da se task pojavi -> dodao sam ja
    this.newTask = new Task();
    this.newTask.summary = this.addTaskForm.controls.t_summary.value;
    this.newTask.description = this.addTaskForm.controls.t_desc.value;
    this.newTask.priority = this.addTaskForm.controls.t_priority.value;
    this.newTask.deadline = this.addTaskForm.controls.t_deadline.value;
    this.newTask.status = 'new';
    this.toggleDisplay();
    //this.board.columns[0].tasks.push(this.newTask);
    this.taskService.insertTask(this.newTask).pipe(first()).subscribe(message => {
    location.reload();

      //promjeni se iteracija!!!

      //ako je uprava ili koordinator onda šalje ID odabranog tima
      /*this.taskService.getTaskBetween(-1, JSON.stringify(this.iterations[this.activeIteration - 1].start).substring(1, 11), JSON.stringify(this.iterations[this.activeIteration - 1].end).substring(1, 11)).pipe(first()).subscribe(tasks => {
        //moraju se ispraznit stupci prije nego se opet napune s dohvacenim taskovima(inace se taskovi krenu duplirat svaki put)
        this.tasksNew = []
        this.tasksIP = []
        this.tasksTesting = []
        this.tasksResolved = []
        for (let i of this.tasks) {
          if ((i.status) == 'new') {
            this.tasksNew.push(i);
          }
          else if ((i.status) == 'in progress') {
            this.tasksIP.push(i);
          }
          else if ((i.status) == 'testing column') {
            this.tasksTesting.push(i);
          }
          else {
            this.tasksResolved.push(i);
          }
        }*/

        //this.tasksTesting.push(this.tasks[0])

        //this.board = new Board('Test Board', [        /* creating the columns with their respective tasks */
         // new Column('New', this.tasksNew),
          //new Column('In progress', this.tasksIP),
          //new Column('Testing Column', this.tasksTesting),
          //new Column('Resolved', this.tasksResolved)
        //])
      //});


    })
  }

  changeTaskStatus() {
    alert('Kako editat task?')
  }

  changeIteration(index: any, teamId?:number) {
    if (!teamId) {
      var teamId=-1
    }
    this.toggledTeamId = teamId;
    this.taskService.getTaskBetween(teamId, JSON.stringify(this.iterations[index - 1].start).substring(1, 11), JSON.stringify(this.iterations[index - 1].end).substring(1, 11)).pipe(first()).subscribe(tasks => {
      this.tasks = tasks;
      //moraju se ispraznit stupci prije nego se opet napune s dohvacenim taskovima(inace se taskovi krenu duplirat svaki put)
      this.tasksNew = []
      this.tasksIP = []
      this.tasksTesting = []
      this.tasksResolved = []
      for (let i of this.tasks) {
        if ((i.status) == 'new') {
          this.tasksNew.push(i);
        }
        else if ((i.status) == 'in progress') {
          this.tasksIP.push(i);
        }
        else if ((i.status) == 'testing column') {
          this.tasksTesting.push(i);
        }
        else {
          this.tasksResolved.push(i);
        }
      }

      //this.tasksTesting.push(this.tasks[0])
      this.board = new Board('Test Board', [        /* creating the columns with their respective tasks */
        new Column('New', this.tasksNew),
        new Column('In progress', this.tasksIP),
        new Column('Testing Column', this.tasksTesting),
        new Column('Resolved', this.tasksResolved)
      ])
    });
    sessionStorage.setItem('activeIteration', index);    
    this.activeIteration = index;
  }

  // toggleTeamSelected(i: any) {
  //   this.teamSelected = true;
  //   this.toggledTeamId=i;
  //   this.taskService.getAll(i).pipe(first()).subscribe(tasks => {
  //     this.tasks = tasks;
  //     //selecting column for task based on status
  //     //moraju se ispraznit stupci prije nego se opet napune s dohvacenim taskovima(inace se taskovi krenu duplirat svaki put)
  //     this.tasksNew = []
  //     this.tasksIP = []
  //     this.tasksTesting = []
  //     this.tasksResolved = []
  //     for (let i of this.tasks) {
  //       if ((i.status) == 'new') {
  //         this.tasksNew.push(i);
  //       }
  //       else if ((i.status) == 'in progress') {
  //         this.tasksIP.push(i);
  //       }
  //       else if ((i.status) == 'testingcolumn') {
  //         this.tasksTesting.push(i);
  //       }
  //       else {
  //         this.tasksResolved.push(i);
  //       }
  //     }
  //     this.board = new Board('Test Board', [        /* creating the columns with their respective tasks */
  //       new Column('New', this.tasksNew),
  //       new Column('In progress', this.tasksIP),
  //       new Column('Testing Column', this.tasksTesting),
  //       new Column('Resolved', this.tasksResolved)
  //     ])
  //   });
  // }

  handleTaskSelect(task: any) {
    this.taskService.getEmployeeForTask(task.idTask).pipe(first()).subscribe(emp=>{
      const modalRef = this.ngbModal.open(TaskModalComponent);
      var cD= new Date(task.creationDate)
      var dl= new Date(task.deadline)
      modalRef.componentInstance.summary=task.summary
      modalRef.componentInstance.description=task.description;
      modalRef.componentInstance.creationDate = {year: cD.getFullYear(), month: cD.getMonth()+1, day: cD.getDate()}
      modalRef.componentInstance.deadline ={year: dl.getFullYear(), month: dl.getMonth()+1, day: dl.getDate()}
      modalRef.componentInstance.status = task.status
      modalRef.componentInstance.idTask=task.idTask.toString()
      modalRef.componentInstance.priority=task.priority.toString()
      modalRef.componentInstance.team=this.teamMembers
      modalRef.componentInstance.assignedId=emp.idEmployee
      modalRef.result.then((result) => {
        if (result) {
          if (result.length==2){
            this.updateTask(result[0])
            this.updateTaskEmployee(result[1].idTask, result[1].idEmployee)
            location.reload();
          } else {
            this.deleteTask(result[0].idTask)
          }
        }
        });
    });

  }

  deleteTask(idTask: any) {
    this.taskService.deleteTask(idTask)
        .pipe(first())
        .subscribe({
            next: () => {
              alert("Task deleted")
              location.reload();
            },  
            error: err => {
              if (err.status == 500)
              alert(err.status)
            }
        });
  }
  updateTaskEmployee(idTask: any, idEmployee: any) {
    this.taskService.updateTaskEmployee(idTask, idEmployee)
        .pipe(first())
        .subscribe({
            next: () => {
            },  
            error: err => {
              if (err.status == 500)
              alert(err.status)

            }
        });
  }

  updateTask(task:any) {
    console.log(task)
    this.taskService.updateTask(task)
        .pipe(first())
        .subscribe({
            next: () => {
            },  
            error: err => {
              if (err.status == 500)
              alert("Cannot update task")
            }
        });
        
  }

  getdate(d) {
    return String(this.pipe.transform(d, 'longDate')); 
  }

}

