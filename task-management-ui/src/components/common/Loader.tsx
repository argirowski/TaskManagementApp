import React from "react";
import "./App.css";
import { Container } from "react-bootstrap";

const Loader: React.FC = () => {
  return (
    <Container className="text-center mt-5">
      <div className="App">
        <h1>Loading...</h1>
      </div>
    </Container>
  );
};

export default Loader;
