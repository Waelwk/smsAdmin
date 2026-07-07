export interface VehiculeList {
  cVehicule: string;
  libVehicule: string;
  numeroSerie: string;
  bActif: boolean;
  hasCTag: boolean;
}

export interface VehiculeDetail {
  cVehicule: string;
  libVehicule: string;
  numeroSerie: string;
  bActif: boolean;
  chargeMax: number;
  bDisponible: boolean;
  chargeLibre: number;
  coutparKM: number;
  cTag: string | null;
  hasCTag: boolean;
}
