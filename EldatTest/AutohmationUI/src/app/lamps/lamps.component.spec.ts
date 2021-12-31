import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LampsComponent } from './lamps.component';

describe('LampsComponent', () => {
  let component: LampsComponent;
  let fixture: ComponentFixture<LampsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LampsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LampsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
