import { Component, OnInit, Input } from '@angular/core';
import { HttpClientModule, HttpClient } from '@angular/common/http';  
import { Router } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import {Character} from '../shared/character';
import{RestApiService} from '../shared/rest-api.service';


@Component({
  selector: 'app-partie1',
  templateUrl: './partie1.component.html',
  styleUrls: ['./partie1.component.css']
})


export class Partie1Component implements OnInit {

 
  Form: FormGroup;
  
  constructor(private fb: FormBuilder,public restApi: RestApiService,private httpService: HttpClient, private router: Router) { }

 
  ngOnInit() {
    this.Form = this.fb.group({  // Crée une instance de FormGroup
      username: [],              // Crée une instance de FormControl
      difficulte: [],            // Crée une instance de FormControl
    });
  }

  @Input() personnageDetails={name:'', difficulte:0}

  nvPartie(dataPersonnage){
    console.log(this.personnageDetails);
    const nomDifficulte = this.personnageDetails.name + this.personnageDetails.difficulte;
    this.restApi.createPersonnage(nomDifficulte).subscribe((data:{}) => {
      console.log(this.personnageDetails);
      this.router.navigate(['/partie11'])
    })
  }

  
}


 

