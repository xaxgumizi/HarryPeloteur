import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';


@Component({
  selector: 'app-fin',
  templateUrl: './fin.component.html',
  styleUrls: ['./fin.component.css']
})
export class FinComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit() {
  }

  demarre(): void{
    this.router.navigate([''])
  }


}
