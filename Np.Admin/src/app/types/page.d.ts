export interface Page {
  pageGuid: string;
  pageName: string;
  routing: string;
  parentPageGuidId: string | null;
  isReadOnly: boolean;
  displayOrder: number;
  pageIcon: string;
  childPages: Page[];
}
