import styled from 'styled-components';

export const StyledHeader = styled.header`
	display:flex;
	align-items:center;
	justify-content:space-between; 
    position: relative;
	z-index:1;
	padding-bottom:15px;
	border-radius:0 0 13px 13px;
	background:linear-gradient(145deg,rgb(21, 63, 74) ,rgb(226, 239, 253));
	box-shadow:2px 5px 10px #37323F,4px -2px 16px #666;

    padding-left: 150px;
    & .menu__anchor{    

        font-weight:500;
        text-decoration: none;
        white-space: nowrap;
        scroll-margin-top: 100px;

        /* underline element onhover*/
        &:hover{ text-decoration: underline; }
        & h3, & h4{
            font-weight:500;
	        white-space:nowrap;
	        scroll-margin-top:100px;
            color: #37323F;
        }
        & h3{
            color: black;
        }
    }

    & .header__nav{
        width: 575px;
        height: inherit;
        display: flex;
        align-items: center;
        justify-content: space-around; 
        all: unset;
        height: inherit;
        & .nav__menu{
            padding-right:250px;
            display:flex;
	        align-items:center;
            justify-content:space-around;
            padding-top:2.5px;
            & .menu__element{
                list-style-type: none;
                & .element__anchor{
                    text-decoration: none;
                    &:hover{ text-decoration: underline; }
                    & h3, & h4{
                        color: #37323F;
                    }
                }
            }
        }
    }
    
`;

