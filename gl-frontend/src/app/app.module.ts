import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule, Routes } from '@angular/router'
import { MainComponent } from './main/main.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './auth.guard';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ProfileComponent } from './profile/profile.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { chainedInstruction } from '@angular/compiler/src/render3/view/util';
import { IjwtInterceptor } from './ijwt.interceptor';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { FullCalendarModule } from '@fullcalendar/angular';
import dayGridPlugin from '@fullcalendar/daygrid'; 
import interactionPlugin from '@fullcalendar/interaction';
import googleCalendarPlugin from '@fullcalendar/google-calendar';
import listPlugin from '@fullcalendar/list';
import timeGridPlugin from '@fullcalendar/timegrid';
import { CalComponent } from './cal/cal.component';
import { HomeComponent } from './home/home.component';
import {PromotionComponent} from './promotion/promotion.component';
import { BoardComponent } from './board/board.component';
import { EmployeesComponent } from './employees/employees.component';
import { TeamsComponent } from './teams/teams.component';
import { GroupsComponent } from './groups/groups.component';
import { NgbModule, NgbDatepicker } from '@ng-bootstrap/ng-bootstrap';
import { MeetingModalComponent } from './cal/meeting-modal/meeting-modal.component';
import { TaskModalComponent } from './main/task-modal/task-modal.component'

FullCalendarModule.registerPlugins([ 
  dayGridPlugin,
  timeGridPlugin,
  listPlugin,
  interactionPlugin,
  googleCalendarPlugin
  
]);

const routes: Routes = [
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
  { path: '', redirectTo: '/home', pathMatch: 'full'},
  { path: 'login', component: LoginComponent },
  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },
  { path: 'changePassword', component: ChangePasswordComponent, canActivate: [AuthGuard] },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: 'cal', component: CalComponent},
  { path: 'main', component: MainComponent },
  { path: 'promote', component: PromotionComponent},
  { path: 'board', component: BoardComponent},
  { path: 'employees', component: EmployeesComponent},
  { path: 'teams', component: TeamsComponent},
  { path: 'groups', component: GroupsComponent}
];

@NgModule({
  declarations: [
    CalComponent, AppComponent, MainComponent, LoginComponent, ProfileComponent, ChangePasswordComponent, ForgotPasswordComponent, HomeComponent, PromotionComponent, BoardComponent, EmployeesComponent, TeamsComponent, GroupsComponent, MeetingModalComponent, TaskModalComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    ReactiveFormsModule,
    DragDropModule,
    BrowserAnimationsModule,
    RouterModule.forRoot(routes, {onSameUrlNavigation: 'reload'}),
    FullCalendarModule,
    FormsModule,
    NgbModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: IjwtInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
