import styled from 'styled-components';

export const StyledFooter = styled.footer`
	text-align: center;
	text-align:center;
	margin:-2px auto auto;
    position: relative;   
    bottom: 0;
    width:100%;
    & .footer__text{ 
        color: #37323F;
    }
    & * {
        position: relative;
        border-radius: 5.5%;
        margin: 0;
        padding: 37.5px;
    
        background:linear-gradient(145deg, rgb(226, 239, 253), rgb(21, 63, 74));
        
	    box-shadow:2px 5px 10px #37323F,4px -2px 16px #666;
    }
`;

