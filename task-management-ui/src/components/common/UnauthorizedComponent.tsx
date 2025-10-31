import React from "react";
import { Container, Row, Col, Card, Button } from "react-bootstrap";
import { useNavigate } from "react-router-dom";

const UnauthorizedComponent: React.FC = () => {
  const navigate = useNavigate();
  return (
    <Container
      className="d-flex align-items-center justify-content-center"
      style={{ minHeight: "100vh" }}
    >
      <Row className="w-100">
        <Col xs={12} sm={10} md={8} lg={6} xl={4} className="mx-auto">
          <Card className="text-center shadow-lg border-0">
            <Card.Body className="p-5">
              <h2 className="mb-3">Unauthorized</h2>
              <p className="mb-4">
                You do not have permission to view this page. Please log in to
                continue.
              </p>
              <Button
                variant="primary"
                size="lg"
                onClick={() => navigate("/login")}
              >
                Go to Login
              </Button>
            </Card.Body>
          </Card>
        </Col>
      </Row>
    </Container>
  );
};

export default UnauthorizedComponent;
