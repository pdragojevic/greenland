import { Injectable } from '@angular/core';
import { GCalEvent } from '../_models/GCalEvent';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { api } from '../api';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CalendarService {
  constructor(private http: HttpClient) {

   }

  getEvents() {
    return this.http.get<any>(`${api.apiUrl}/Calendar/events`).pipe(map(resp => {
      return resp.items.map(item => {return this.gcalItemToRawEventDef(item)})
    }));
  }

  insertEvent(event) {
    const body = JSON.stringify(this.RawEventDefTogcalItem(event));
        let headers = new HttpHeaders();
        headers = headers.set('Content-Type', 'application/json; charset=utf-8');   

        return this.http.post<any>(`${api.apiUrl}/Calendar/insert`, body, {headers: headers})
            .pipe(map(x => { 
              return x;
            }));
  }

  gcalItemToRawEventDef(item) {
    return {
        id: item.id,
        title: item.summary,
        start: item.start.dateTime || item.start.date,
        end: item.end.dateTime || item.end.date,
        location: item.location,
        description: item.description
    };
  }

  //{"summary":"test","start":{"dateTime":"2021-01-05T14:00:00+01:00"},"end":{"dateTime":"2021-01-07T15:00:00+01:00"},"location":"fer","description":"test123"}
    RawEventDefTogcalItem(item) {
      return {
          summary: item.title,
          description: item.description,
          start: {
            dateTime: item.start
          },
          end: {
            dateTime: item.end
          },
          location: item.location,
          attendees: item.attendees
      };
}
  
}

