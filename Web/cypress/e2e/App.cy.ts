import Content from '../../src/model/content';
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

  it('should store planet details in local storage after the first API call', () => {
    // Make a planet selection to trigger the API call
    homePage.selectPlanet('Earth');

    // Wait for the API call to complete and assert the stored planet details
    cy.wait(1000).then(() => {
      cy.window().then((window) => {
        homePage.getSelectedPlanetName().invoke('attr', 'data-testid').then((id) => {
          const storedDetails = window.localStorage.getItem(`planetDetails-${id}`);
        
          // eslint-disable-next-line @typescript-eslint/no-unused-expressions
          expect(storedDetails).not.to.be.null;
  
          expect(JSON.parse(String(storedDetails))).to.have.property('name', 'Earth');
          expect(JSON.parse(String(storedDetails))).to.have.property('diameterKilometers', 12742);
          expect(JSON.parse(String(storedDetails))).to.have.property('massTonnes', 5973.6);
        });
      });
    });
  });

  it('should retrieve planet details from local storage instead of making API requests', () => {
    // Set the planet details in local storage manually
    const planetDetails = {
      id: 1,
      name: 'Earth',
      diameter: 12742,
      mass: '5.972 × 10^24 kg',
    };
    cy.window().its('localStorage').invoke('setItem', 'planetDetails-1', JSON.stringify(planetDetails));

    // Select the planet and check if the details are retrieved from local storage
    homePage.selectPlanet('Earth');

    cy.fixture('earth.json').then((data: {diameter:string, mass:string}) => {
      homePage.getPlanetDetailsSection().should(($details) => {
        expect($details).to.contain(`${ReadableKeys["diameterKilometers"]}: ${data.diameter}`);
        expect($details).to.contain(`${ReadableKeys["massTonnes"]}: ${data.mass}`);
      });
    });
  });

  it('should display the footer text correctly', () => {
    homePage.getFooterText().should('contain', Content.footerText);
  });
});