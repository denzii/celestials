import styled from "styled-components";

export const StyledHero = styled.section`
    height: 85.0vh;
    margin-top:-10px;
    z-index: 0;
    background-position: center;
    background-image: url("hero.jpg");
    background-size: cover;
    background-repeat: no-repeat;
    background-attachment: fixed;
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: row;
    flex-wrap: nowrap;
    width: 100vw;
    & .hero__cta{
        width:100%;
        height: 100%;
        display: flex;
        flex-direction: row;
        justify-content: center;
        position: relative;
        z-index: 0;
        background: rgba(255, 255, 255, 0.1);
        backdrop-filter: blur(3px);
        padding: 20px;
        font-size: 22.5px;
        & .cta__content{
            display: flex;
            flex-direction: column;
            justify-content: center;
        }
        & .hero__supplementary{
        color: white;
    }
    }

    & h1, h2, h3{
        font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif;
        z-index: 1;
        margin: 5;
    } 
`