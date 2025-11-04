import React, { useEffect, useState } from "react";
import { Container, Navbar, Nav, Button } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { clearToken } from "../../utils/auth";

const NavigationComponent: React.FC = () => {
  const navigate = useNavigate();

  const [userName, setUserName] = useState<string | null>(null);

  useEffect(() => {
    // Try to get the user name from localStorage (adjust key as needed)
    const storedName = localStorage.getItem("userName");
    setUserName(storedName);
  }, []);

  const handleLogout = () => {
    clearToken();
    navigate("/");
  };

  return (
    <Navbar bg="dark" variant="dark" expand="lg" className="mb-4">
      <Container>
        <Navbar.Brand href="/projects">Task Management</Navbar.Brand>
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="me-auto">
            <Nav.Link href="/projects">Projects</Nav.Link>
          </Nav>
          <Nav className="ms-auto">
            <Navbar.Text className="me-3">
              {userName ? `Welcome, ${userName}!` : "Welcome!"}
            </Navbar.Text>

            <Button variant="outline-light" size="sm" onClick={handleLogout}>
              Logout
            </Button>
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
};

export default NavigationComponent;
