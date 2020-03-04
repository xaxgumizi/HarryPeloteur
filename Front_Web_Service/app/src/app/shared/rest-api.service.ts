import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Character } from '../shared/character';
import { Observable, throwError } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RestApiService {

  apiUrl = 'https://localhost:44344/api';
  id = 36;

  constructor(private http: HttpClient) { }

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type':'application/json'
    })
  }

 



  //Ajouter personnage
  createPersonnage(character):Observable<Character>{
    const t = '"+ character +"';
    return this.http.post<Character>(this.apiUrl+'/newGame',character,this.httpOptions)
    .pipe(
      retry(1)
    )
  }

  //Fuir
  FuirPersonnage(character):Observable<Character>{
    const body = '"fuir"';
    return this.http.put<Character>(this.apiUrl+'/game/'+this.id,body,this.httpOptions)
    .pipe(
      retry(1)
    )
  }

  //Combattre
  CombattreMonstre(character):Observable<Character>{
    const body = '"combattre"';
    return this.http.put<Character>(this.apiUrl+'/game/'+this.id,body,this.httpOptions)
    .pipe(
      retry(1)
    )
  }

  //Ramasser
  RamasserObjet(character):Observable<Character>{
    const body = '"ramasser"';
    return this.http.put<Character>(this.apiUrl+'/game/'+this.id,body,this.httpOptions)
    .pipe(
      retry(1)
    )
  }

}
