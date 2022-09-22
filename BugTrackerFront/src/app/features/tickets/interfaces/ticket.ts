export interface Ticket {
  id: number;
  subject: string;
  application: number;
  state: string;
  creatorId: string;
  ownerId: string;
  creationDate: Date;
  description: string,
  screenshot: Uint8Array;
}
