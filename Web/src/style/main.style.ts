import styled from "styled-components";

export const StyledMain = styled.main`
  height: 90vh;
  border-radius: 0 0 13px 13px;
  background: linear-gradient(190deg, rgb(7, 45, 60), rgb(21, 63, 74));
  box-shadow: 2px 3px 4px #37323f, 4px -2px 10px rgb(35, 30, 38);
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: space-around;

  & p:last-of-type {
    margin-bottom: 0;
  }

  & .main__lhs {
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    justify-content: flex-start;

    & .lhs__list {
      /* Define styles for lhs list */
    }

    & .list__elem {
        // add onhover effect
        padding:10px;
        color: beige;
        &:hover {
            background-color: #37323F;
            color: grey;
            cursor: pointer;
        }
      /* Define styles for list items */
    }
  }

  & .main__rhs {


    & .rhs__data-header {
      /* Define styles for rhs data header */
    }

    & .rhs__planet {
      /* Define styles for rhs planet container */

      & .planet__img {
        /* Define styles for planet image */
      }

      & .planet__detail {
        /* Define styles for planet details */
      }
    }
  }
`;
