import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorklogReportFilter } from './worklog-report-filter';

describe('WorklogReportFilter', () => {
  let component: WorklogReportFilter;
  let fixture: ComponentFixture<WorklogReportFilter>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WorklogReportFilter],
    }).compileComponents();

    fixture = TestBed.createComponent(WorklogReportFilter);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
