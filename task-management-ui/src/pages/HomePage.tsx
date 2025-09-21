import React from "react";
import "./App.css";
import { Container } from "react-bootstrap";

const HomePage: React.FC = () => {
  return (
    <Container className="text-center mt-5">
      <div className="App">
        <h1>Landing Page</h1>
      </div>
    </Container>
  );
};

export default HomePage;
