import React, { useState } from "react";
import { Container, Nav, Navbar } from "react-bootstrap";
import LogInUser from "./features/auth/LogInUser";
import RegisterUser from "./features/auth/RegisterUser";
import "bootstrap/dist/css/bootstrap.min.css";
import "./App.css";

function App() {
  const [activeView, setActiveView] = useState<"login" | "register">("login");

  const handleLogin = (email: string, password: string) => {
    console.log("Login attempt:", { email, password });
    // Add your login logic here
    alert(`Login attempt for: ${email}`);
  };

  const handleRegister = (userData: any) => {
    console.log("Registration attempt:", userData);
    // Add your registration logic here
    alert(`Registration attempt for: ${userData.email}`);
  };

  return (
    <div className="App">
      <Navbar bg="dark" variant="dark" expand="lg">
        <Container>
          <Navbar.Brand href="#home">Task Management App</Navbar.Brand>
          <Nav className="me-auto">
            <Nav.Link
              onClick={() => setActiveView("login")}
              active={activeView === "login"}
            >
              Login
            </Nav.Link>
            <Nav.Link
              onClick={() => setActiveView("register")}
              active={activeView === "register"}
            >
              Register
            </Nav.Link>
          </Nav>
        </Container>
      </Navbar>

      <div className="main-content">
        {activeView === "login" ? (
          <LogInUser onLogin={handleLogin} />
        ) : (
          <RegisterUser onRegister={handleRegister} />
        )}
      </div>
    </div>
  );
}

export default App;
