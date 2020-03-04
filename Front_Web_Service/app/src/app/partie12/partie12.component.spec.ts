import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Partie12Component } from './partie12.component';

describe('Partie12Component', () => {
  let component: Partie12Component;
  let fixture: ComponentFixture<Partie12Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Partie12Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Partie12Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
