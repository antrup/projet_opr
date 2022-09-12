import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from '../material.module';
import { RouterTestingModule } from '@angular/router/testing';
import { of } from 'rxjs';
import { TicketsComponent } from './tickets.component';
import { Ticket } from './interfaces/ticket';
import { TicketService } from './ticket.service';
import { ApiResult } from '../base.service';
import { Application } from '../shared-services/interfaces/application';

import { AuthService } from '../auth/auth.service';

// Test of ticketsComponent

describe('TicketsComponent', () => {
  let component: TicketsComponent;
  let fixture: ComponentFixture<TicketsComponent>;

  beforeEach(async () => {
    // Create a mock TicketService object with mock 'getData and loadApplications' method
    let ticketServiceStub = jasmine.createSpyObj<TicketService>('TicketService', ['getData', 'loadApplications', 'applications']);

    ticketServiceStub.getData.and.returnValue(
      // return an Observable with some test data
      of<ApiResult<Ticket>>(<ApiResult<Ticket>>{
        data: [
          <Ticket>{
            id: 1,
            subject: "A",
            application: 1,
            state: "new",
            creatorId: "fakeUser",
            ownerId: "",
            creationDate: new Date(),
            description: "test test test",
            screenshot: new Uint8Array(),
          },
          <Ticket>{
            id: 2,
            subject: "B",
            application: 1,
            state: "new",
            creatorId: "fakeUser",
            ownerId: "",
            creationDate: new Date(),
            description: "test test test",
            screenshot: new Uint8Array(),
          },
          <Ticket>{
            id: 3,
            subject: "C",
            application: 1,
            state: "new",
            creatorId: "fakeUser",
            ownerId: "",
            creationDate: new Date(),
            description: "test test test",
            screenshot: new Uint8Array(),
          },
        ],
        totalCount: 3,
        pageIndex: 0,
        pageSize: 10
      }));

    ticketServiceStub.loadApplications.and.returnValue(Promise.resolve());

    ticketServiceStub.applications = [<Application>{

      id: 1,
      name: "testApp",
    }];

     // Create a mock TicketService object with a mock 'isDevUser' method
    let authServiceStub = jasmine.createSpyObj<AuthService>('AuthService', ['isDevUser']);

    authServiceStub.isDevUser.and.returnValue(true);

    await TestBed.configureTestingModule({
      declarations: [TicketsComponent],
      imports: [
        BrowserAnimationsModule,
        MaterialModule,
        RouterTestingModule,
      ],
      providers: [
        {
          provide: AuthService,
          useValue: authServiceStub
        }
      ]
    })
      // As tickerService is injected at Component level, spy must be implemented with overridecomponent
      .overrideComponent(TicketsComponent,
        {
          set: {
            providers: [
              {
                provide: TicketService,
                useValue: ticketServiceStub
              }
            ]
          }
        })
      .compileComponents();
  });

  beforeEach(async () => {
    fixture = TestBed.createComponent(TicketsComponent);
    component = fixture.componentInstance;
    component.paginator = jasmine.createSpyObj(
      "MatPaginator", ["length", "pageIndex", "pageSize"]
    );
    fixture.autoDetectChanges();
    await fixture.componentInstance.loadData();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should populate application map', () => {
    expect(component.applicationsMap.size).toBeGreaterThan(0);
  })

  it('should import data into datasource', () => {
    expect(component.dataSource).toBeDefined()
  });

  it('should contain a table with a list of one or more tickets', () => {
    let table = fixture.nativeElement
      .querySelector('table.mat-table');
    let tableRows = table
      .querySelectorAll('tr.mat-row');
    expect(tableRows.length).toBeGreaterThan(0);
  });
})
