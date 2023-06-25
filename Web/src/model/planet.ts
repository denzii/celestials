interface BaseEntity {
    id: number;
    name: string;
}

type Planet = BaseEntity & {
    photoUrl: string | null;
    distanceFromSunKilometers: number;
    massTonnes: number;
    diameterKilometers: number;
    lengthOfDayHours: number;
}

export default BaseEntity;
export type { Planet };