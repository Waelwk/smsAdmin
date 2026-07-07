/** DTO liste chauffeur — aligné avec EmployeListDto backend */
export interface EmployeList {
  matricule: string;
  nomPrenom: string | null;
  bActif: boolean | null;
  hasPassword: boolean;
  hasCTag: boolean;
}

/** DTO détail chauffeur — aligné avec EmployeDetailDto backend */
export interface EmployeDetail {
  matricule: string;
  nomPrenom: string | null;
  chargeParHeure: number | null;
  bActif: boolean | null;
  typeEmp: string | null;
  bResponsable: boolean | null;
  cEquipe: string | null;
  tagC: string | null;
  cPosteEmployer: string | null;
  cSociete: string | null;
  cSite: string | null;
  banned: boolean | null;
  hasPassword: boolean;
  hasCTag: boolean;
}
