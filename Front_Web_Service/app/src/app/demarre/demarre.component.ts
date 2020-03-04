import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-demarre',
  templateUrl: './demarre.component.html',
  styleUrls: ['./demarre.component.css']
})
export class DemarreComponent implements OnInit {

  constructor(private router: Router) { }

  onClickN(): void{
    this.router.navigate(['/partie1']);
  }
  onClickC():void{
    this.router.navigate(['/continue']);
  }
  onClickA():void{
    this.router.navigate(['/admin']);
  }


  ngOnInit() {
  }

}
