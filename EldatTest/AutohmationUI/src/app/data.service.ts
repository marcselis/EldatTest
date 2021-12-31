import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  constructor(private http: HttpClient) { }

  getLamps() : Observable<Lamp[]>
{
    return this.http.get<Lamp[]>('/api/lamps')
      .pipe(tap(response => console.log(response)));
    //return this.http.get('https://jsonplaceholder.typicode.com/users');
    
  }

  
}

export class Action {

  
  private _description : string;
  public get description() : string {
    return this._description;
  }
  public set description(v : string) {
    this._description = v;
  }
  
  
  private _verb : string;
  public get verb() : string {
    return this._verb;
  }
  public set verb(v : string) {
    this._verb = v;
  }
  
  
  private _link : string;
  public get link() : string {
    return this._link;
  }
  public set link(v : string) {
    this._link = v;
  }
  
}

export class Lamp {
  
  private _name : string;
  public get name() : string {
    return this._name;
  }
  public set name(v : string) {
    this._name = v;
  }
  
  
  private _description : string;
  public get description() : string {
    return this._description;
  }
  public set description(v : string) {
    this._description = v;
  }
  
  private _state : number;
  public get state() : number {
    return this._state;
  }
  public set state(v : number) {
    this._state = v;
  }
  
  
  private _level : number;
  public get level() : number {
    return this._level;
  }
  public set level(v : number) {
    this._level = v;
  }
  
  
  private _actions : Action[];
  public get actions() : Action[] {
    return this._actions;
  }
  public set actions(v : Action[]) {
    this._actions = v;
  }
  }

