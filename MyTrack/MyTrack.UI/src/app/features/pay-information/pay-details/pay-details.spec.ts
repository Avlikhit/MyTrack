import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PayDetails } from './pay-details';

describe('PayDetails', () => {
  let component: PayDetails;
  let fixture: ComponentFixture<PayDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PayDetails],
    }).compileComponents();

    fixture = TestBed.createComponent(PayDetails);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
