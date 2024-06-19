export class AccessRights {
  pageRoute!: string;
  pageName!: string;
  hasRights!: boolean

  constructor(pagename: string, pageRoute: string, hasRights: boolean) {
    this.pageName = pagename;
    this.pageRoute = pageRoute;
    this.hasRights = hasRights;
  }
}
