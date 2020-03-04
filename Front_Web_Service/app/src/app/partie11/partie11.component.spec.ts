import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Partie11Component } from './partie11.component';

describe('Partie11Component', () => {
  let component: Partie11Component;
  let fixture: ComponentFixture<Partie11Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Partie11Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Partie11Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
