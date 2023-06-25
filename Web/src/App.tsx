import './App.css';

import React, { useEffect, useState } from 'react';
import { StyledHeader } from './style/header.style';
import { StyledHero } from './style/hero.style';
import { StyledMain } from './style/main.style';
import { StyledFooter } from './style/footer.style';
import _ from './model/view/keyableFragment';
import BaseEntity, { Planet } from './model/planet';
import ReadableKeys from './model/planetKeyMap';
import Content from './model/content';

function App() {
  const [planets, setPlanets] = useState<BaseEntity[]>([]);
  const [selectedPlanet, setSelectedPlanet] = useState<BaseEntity | null>(null);
  const [planetDetails, setPlanetDetails] = useState<Planet | null>(null);

  useEffect(() => {
    const fetchPlanets = async () => {
      try {
        const response = await fetch('http://localhost:8081/Planet/GetAll');
        const data = await response.json();
        setPlanets(data);
      } catch (error) {
        console.error('Error fetching planets:', error);
      }
    };

    fetchPlanets();
  }, []);

  const fetchPlanetDetails = async (planetId: number) => {
    const storedPlanetDetails = localStorage.getItem(`planetDetails-${planetId}`);

    if (storedPlanetDetails) {
      console.log("Retrieving planet data from local storage...");

      setPlanetDetails(JSON.parse(storedPlanetDetails));
    } else {
      try {
        console.info("Fetching planet data from api...");
        const response = await fetch(`http://localhost:8081/Planet/Get?id=${planetId}`);
        const data = await response.json();
        setPlanetDetails(data);
        localStorage.setItem(`planetDetails-${planetId}`, JSON.stringify(data));
      } catch (error) {
        console.error('Error fetching planet details:', error);
      }
    }
  };

  const handlePlanetClick = (planet: { name: string; id: number }) => {
    setSelectedPlanet(planet);
    fetchPlanetDetails(planet.id);
  };


  return (
    <>
      <div className="app">
        <StyledHeader role="banner" className="app__header" aria-labelledby="header__description">
          <h2 className="header__description hidden">{Content.headerDescription}</h2>
          <a className="menu__anchor" href="/#" title="Scroll to Top">
            <h3>Inclusive Technology</h3>
          </a>
          <nav className="header__nav">
            <ul className="nav__menu">
              {["Demo"].map((key: string) => (
                <_ key={`menu__element-${key}`}>
                  <li className="menu__element">
                    <a className="element__anchor" title={`Scroll to ${key} section`} href={`#${key}`}>
                      <h4>{key}</h4>
                    </a>
                  </li>
                </_>
              ))}
            </ul>
          </nav>
        </StyledHeader>
        <hr />
        <StyledHero role="img" className="app__hero" aria-labelledby="hero__description">
          <h2 className="hero__description hidden" id="hero__description">
            {Content.heroDescription}
          </h2>
          <article className="hero__cta">
            <div role="presentation" className="cta__content">
              <h1 className="hero__title">{Content.heroTitle}</h1>
              <h3 className="hero__body">{Content.heroBody}</h3>
              <ol className="hero__supplementary">
                {Content.heroSupplementary.map((item, index) => (
                  <li key={index}>{item}</li>
                ))}
              </ol>
            </div>
          </article>
        </StyledHero>
        <hr />
        <StyledMain className="app__main" id="Demo" aria-labelledby="demo__description">
          <section className="main__lhs">
            <ol className="lhs__list">
              {planets.map((planet) => (
                <li className="list__elem" key={planet.id} onClick={() => handlePlanetClick(planet)}>
                  {planet.name}
                </li>
              ))}
            </ol>
          </section>
          {selectedPlanet && (
            <section className="main__rhs">
              <h2 className="rhs__data-header" data-testid={selectedPlanet.id}>
                {selectedPlanet.name}
              </h2>
              {planetDetails && (
                <div className="rhs__planet">
                  {Object.entries(planetDetails).map(([key, value]) =>
                    key === 'photoUrl' ? (
                      <img className="planet__img" src={String(value)} alt={selectedPlanet.name} key={key} />
                    ) : key === 'id' || key === 'name' ? null : (
                      <p key={key} className={`planet__detail`}>
                        {ReadableKeys[key as keyof Omit<Planet, 'id'>]}: {value}
                      </p>
                    )
                  )}
                </div>
              )}
            </section>
          )}
        </StyledMain>
        <hr />
        <StyledFooter role="banner" className="app__footer" aria-labelledby="footer__description">
          <h2 className="footer__description hidden">{Content.footerDescription}</h2>
          <p className="footer__text">{Content.footerText}</p>
        </StyledFooter>
      </div>
    </>
  );
}

export default App;
