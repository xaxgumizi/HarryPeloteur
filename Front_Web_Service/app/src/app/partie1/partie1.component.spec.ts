import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Partie1Component } from './partie1.component';

describe('Partie1Component', () => {
  let component: Partie1Component;
  let fixture: ComponentFixture<Partie1Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Partie1Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Partie1Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
