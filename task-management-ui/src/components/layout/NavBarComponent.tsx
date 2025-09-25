import React from "react";
import "./App.css";
import { Container } from "react-bootstrap";

const NavBarComponent: React.FC = () => {
  return (
    <Container className="text-center mt-5">
      <div className="App">
        <h1>NavBar</h1>
      </div>
    </Container>
  );
};

export default NavBarComponent;
