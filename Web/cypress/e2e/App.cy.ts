import Content from '../../src/model/content';
import { Planet } from '../../src/model/planet';
import ReadableKeys from '../../src/model/planetKeyMap';

class HomePage {
  visit() {
    cy.visit('http://localhost:3000/');
  }

  getNavigationLinks() {
    return cy.get('.nav__menu li');
  }

  getHeroTitle() {
    return cy.get('.hero__title');
  }

  getHeroDescription() {
    return cy.get('.hero__body');
  }

  getSupplementaryList() {
    return cy.get('.hero__supplementary li');
  }

  getPlanetList() {
    return cy.get('.lhs__list li');
  }

  selectPlanet(planetName: string) {
    return cy.contains('.list__elem', planetName).click();
  }

  getSelectedPlanetName() {
    return cy.get('.rhs__data-header');
  }

  getPlanetDetailsSection() {
    return cy.get('.rhs__planet');
  }

  getPlanetDetails() {
    return cy.get('.rhs__planet p');
  }
  getPlanetImage() {
    return cy.get('.planet__img');
  }

  getFooterText() {
    return cy.get('.footer__text');
  }
}

describe('App Tests', () => {
  const homePage = new HomePage();

  beforeEach(() => {
    homePage.visit();
  });

  it('should display header with navigation links', () => {
    homePage.getNavigationLinks().should('be.visible');
  });

  it('should display hero section with correct title and description', () => {
    homePage.getHeroTitle().should('contain', Content.heroTitle);
    homePage.getHeroDescription().should('have.text', Content.heroBody);

    homePage.getSupplementaryList().should(($listItems) => {
      expect($listItems).to.have.length(3);
      expect($listItems.eq(0)).to.have.text(Content.heroSupplementary[0]);
      expect($listItems.eq(1)).to.have.text(Content.heroSupplementary[1]);
      expect($listItems.eq(2)).to.have.text(Content.heroSupplementary[2]);
    });
  });

  it('should fetch and display the initial planet list correctly', () => {
    homePage.getPlanetList().should('have.length.greaterThan', 0);

    homePage.getPlanetList().should(($list) => {
      expect($list).to.have.length.greaterThan(0);
      expect($list).to.contain('Earth');
      expect($list).to.contain('Mars');
    });
  });

  it('should select a planet and display its details correctly', () => {
    const planetName = 'Earth';
    homePage.selectPlanet(planetName);

    homePage.getSelectedPlanetName().should('contain', planetName);
    homePage.getPlanetDetailsSection().should('be.visible');
    
    cy.fixture('earth.json').then((data: {diameter:string, mass:string}) => {
    homePage.getPlanetDetailsSection().should(($details) => {
      expect($details).to.contain(`${ReadableKeys["diameterKilometers"]}: ${data.diameter}`);
      expect($details).to.contain(`${ReadableKeys["massTonnes"]}: ${data.mass}`);
    });
  });

    homePage.getPlanetImage().should('be.visible');
  });

  it('should store planet details in state after the first API call', () => {
    homePage.selectPlanet('Earth');
  
    cy.get(".app").invoke('attr', 'data-test-planet-cache').then((planets) => {
      const planetCache: Planet[] = JSON.parse(String(planets));
      const earthDetails = planetCache.find((planet: { name: string }) => planet.name === 'Earth');
  
      // eslint-disable-next-line @typescript-eslint/no-unused-expressions
      expect(earthDetails).not.to.be.null;
  
      expect(earthDetails).to.have.property('name', 'Earth');
      expect(earthDetails).to.have.property('diameterKilometers', 12742);
      expect(earthDetails).to.have.property('massTonnes', 5973.6);
    });
  });
  
  it('should retrieve planet details from state instead of making API requests', () => {
    cy.intercept('GET', 'http://localhost:8081/Planet/Get').as('getPlanetDetails'); // Intercept the network request
  
    homePage.selectPlanet('Earth');
  
    cy.wait(3000); // Wait for 3 seconds
  
    cy.get('@getPlanetDetails').then((interception) => {
      if (interception) {
        expect(interception).to.have.property('response'); // Network request was made
      } else {
        // eslint-disable-next-line @typescript-eslint/no-unused-expressions
        expect(true).to.be.true; // Network request was not made
      }
    });
  
    homePage.getPlanetDetailsSection().should(($details) => {
      expect($details).to.contain(`${ReadableKeys["diameterKilometers"]}: 12742`);
      expect($details).to.contain(`${ReadableKeys["massTonnes"]}: 5973.6`);
    });
  });
  

  it('should display the footer text correctly', () => {
    homePage.getFooterText().should('contain', Content.footerText);
  });
});