import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PayInformation } from './pay-information';

describe('PayInformation', () => {
  let component: PayInformation;
  let fixture: ComponentFixture<PayInformation>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PayInformation],
    }).compileComponents();

    fixture = TestBed.createComponent(PayInformation);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
