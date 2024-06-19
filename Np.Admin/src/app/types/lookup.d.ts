export interface Lookup {
  lookupGuid: string;
  name: string;
  description: string;
  parentLookupId?: string;
  isActive: boolean;
  isDeleted: boolean;
  childValues: LookupValues[];
}

export interface LookupValues {
  lookupValueGuid: string;
  name: string;
  optionValues: string;
  optionTooltip?: string;
  isActive: boolean;
  isDeleted: boolean;
  lookupGuid: string;
}
