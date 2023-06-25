import styled from "styled-components";

export const StyledDataList = styled.form`
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 1rem;
  div {
    display: flex;
    flex-direction: column;
    margin-bottom: 1rem;
  }

  label {
    font-weight: bold;
    margin-bottom: 0.5rem;
  }

  input[type='file'] {
    padding: 0.5rem;
    border-radius: 4px;
    border: 1px solid gray;
  }

  button[type='submit'] {
    padding: 0.75rem 1.5rem;
    background-color:black;
    color: white;
    border-radius: 4px;
    border: solid black;
    cursor: pointer;
  }
`;