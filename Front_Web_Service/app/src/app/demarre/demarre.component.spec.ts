import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DemarreComponent } from './demarre.component';

describe('DemarreComponent', () => {
  let component: DemarreComponent;
  let fixture: ComponentFixture<DemarreComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DemarreComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DemarreComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
