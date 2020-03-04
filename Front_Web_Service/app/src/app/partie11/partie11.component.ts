import { Component, OnInit } from '@angular/core';
import { HttpClientModule, HttpClient, HttpHeaders } from '@angular/common/http';  
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import {Character} from '../shared/character';
import{RestApiService} from '../shared/rest-api.service';

@Component({
  selector: 'app-partie11',
  templateUrl: './partie11.component.html',
  styleUrls: ['./partie11.component.css']
})
export class Partie11Component implements OnInit {

  constructor(private httpService: HttpClient, private router: Router, public restApi:RestApiService) { }

  id = 37;
  taille: number =  2 ;
  games: string[]; 
  gameInfos: string[];
  character: string[];
  rooms: string[];
  text: string[];
  Resultattext: string[];
  body: string = 'debut';
  url: string;
  actions: string[];
  t:string[];
  endText: string[];
  
  
  ngOnInit() {
   
    if(this.body=="debut"){
      console.log(this.body);
        this.url = 'https://localhost:44344/api/game/'+this.id;
        this.body = '"avancer"';
        const head = new HttpHeaders().set('Content-Type', 'application/json');
        this.httpService.put(this.url,this.body,{headers:head}).
        subscribe(  
          data => {  
            this.games = data as string[];
            this.text = this.games['resultText'];
            this.t = this.games['text'];
            this.gameInfos = this.games['gameInfos'];
            this.character = this.gameInfos['Character'];  
            this.rooms = this.gameInfos['Rooms'];
            this.actions = this.t['actionsPossibles'];
            this.endText = this.t['endText'];
            this.taille = 5;
          }  
        );  
    }
    else{
        this.url = 'https://localhost:44344/api/game/'+this.id;
        const header = new HttpHeaders().set('Content-Type', 'application/json');
        this.httpService.put(this.url,this.body,{headers:header}).
        subscribe(  
          data => {  
            this.games = data as string[];
            this.text = this.games['resultText'];
            this.gameInfos = this.games['gameInfos'];
            this.character = this.gameInfos['Character'];  
            this.rooms = this.gameInfos['Rooms'];
            this.actions = this.t['actionsPossibles'];
            this.endText = this.t['endText'];
            this.taille = 5;
          }  
        ); 
    }
    

    //Canevas pour Map
    var canvas : any = document.getElementById("myCanvas");
    if(canvas.getContext){
      var ctx = canvas.getContext("2d");
      var cercle = canvas.getContext("2d");
      var porte = canvas.getContext("2d");
      var x = 0;
      var y = 0;
      
      //map
      for(var i = 0; i < this.taille; i++) {
        ctx.beginPath();
        ctx.fillRect(x, y, 50, 50);
        x = x + 50;
        ctx.fillStyle = "";
      }

      var xCercle = 20;
      var yCrrcle = 20;
      //cercle
      cercle.beginPath();
      cercle.fillStyle="#FF4422"
      cercle.arc(xCercle,yCrrcle,10,0,2*Math.PI);
      cercle.fill();
    }
    
  }

  Avancer(): void{
      this.body = '"avancer"';
      this.router.navigate(['/partie12']);
  }

  Combattre(): void{
    this.body = '"combattre"';
    this.restApi.CombattreMonstre(this.character).subscribe((data:{}) => {
      this.games = data as string[];
      this.text = this.games['resultText'];
      this.actions = this.t['actionsPossibles'];
      this.endText = this.t['endText'];
      if(this.character['Pv'] < 0)
        this.router.navigate(['/fin'])
      else
        this.router.navigate(['/partie11'])
    })
    
  }
 
  Fuir(): void{
    this.body = '"fuir"';
    this.restApi.FuirPersonnage(this.character).subscribe((data:{}) => {
      this.games = data as string[];
      this.text = this.games['resultText'];
      this.actions = this.t['actionsPossibles'];
      this.endText = this.t['endText'];
      this.router.navigate(['/partie12'])
    })
  }

  Ramasser(): void{
    this.body = '"ramasser"';
    this.restApi.RamasserObjet(this.character).subscribe((data:{}) => {
      this.games = data as string[];
      this.text = this.games['resultText'];
      this.actions = this.t['actionsPossibles'];
      this.endText = this.t['endText'];
      this.router.navigate(['/partie11'])
    })
  }
  

}
