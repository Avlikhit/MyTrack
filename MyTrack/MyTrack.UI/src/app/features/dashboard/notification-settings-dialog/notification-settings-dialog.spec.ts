import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NotificationSettingsDialog } from './notification-settings-dialog';

describe('NotificationSettingsDialog', () => {
  let component: NotificationSettingsDialog;
  let fixture: ComponentFixture<NotificationSettingsDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NotificationSettingsDialog],
    }).compileComponents();

    fixture = TestBed.createComponent(NotificationSettingsDialog);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
