import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorklogDetails } from './worklog-details';

describe('WorklogDetails', () => {
  let component: WorklogDetails;
  let fixture: ComponentFixture<WorklogDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WorklogDetails],
    }).compileComponents();

    fixture = TestBed.createComponent(WorklogDetails);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
