import { Planet } from "./planet";


type PlanetKeyMap = {
    [key in keyof Planet]: string;
};

const ReadableKeys: Omit<PlanetKeyMap, "id"> = {
    name: "Name",
    diameterKilometers: "Planet Diameter in Kilometers",
    distanceFromSunKilometers: "Distance from the Sun in Kilometers",
    lengthOfDayHours: "Length of Day in Hours",
    massTonnes: "Mass in Tonnes",
    photoUrl: "Photo Url",
  };
  

export default ReadableKeys;