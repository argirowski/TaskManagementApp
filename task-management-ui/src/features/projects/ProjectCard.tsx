import React, { useState, useEffect } from "react";
import {
  Container,
  Row,
  Col,
  Card,
  Button,
  Alert,
  Spinner,
  Badge,
  ListGroup,
} from "react-bootstrap";
import { useNavigate, useParams } from "react-router-dom";
import axios from "axios";
import { ProjectDetailsDTO } from "../../types/types";

const ProjectCard: React.FC = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();

  const [project, setProject] = useState<ProjectDetailsDTO | null>(null);
  const [loading, setLoading] = useState(true);
  const [showAlert, setShowAlert] = useState(false);
  const [alertMessage, setAlertMessage] = useState("");
  const [alertVariant, setAlertVariant] = useState<"success" | "danger">(
    "success"
  );

  useEffect(() => {
    if (id) {
      fetchProject(id);
    }
  }, [id]);

  const fetchProject = async (id: string) => {
    try {
      setLoading(true);
      const response = await axios.get(
        `https://localhost:7272/api/Projects/${id}`
      );
      setProject(response.data);
    } catch (error: any) {
      console.error("Error fetching project:", error);
      setAlertMessage("Failed to load project details. Please try again.");
      setAlertVariant("danger");
      setShowAlert(true);
      setTimeout(() => setShowAlert(false), 5000);
    } finally {
      setLoading(false);
    }
  };

  const handleBackToProjects = () => {
    navigate("/projects");
  };

  const handleEditProject = () => {
    navigate(`/projects/${id}/edit`);
  };

  if (loading) {
    return (
      <Container
        className="d-flex justify-content-center align-items-center"
        style={{ minHeight: "100vh" }}
      >
        <div className="text-center">
          <Spinner animation="border" variant="primary" />
          <p className="mt-3">Loading project details...</p>
        </div>
      </Container>
    );
  }

  if (!project) {
    return (
      <Container className="mt-4">
        <Alert variant="danger">
          <Alert.Heading>Project Not Found</Alert.Heading>
          <p>The project you're looking for could not be found.</p>
          <Button variant="primary" onClick={handleBackToProjects}>
            Back to Projects
          </Button>
        </Alert>
      </Container>
    );
  }

  return (
    <Container>
      {showAlert && (
        <Alert
          variant={alertVariant}
          dismissible
          onClose={() => setShowAlert(false)}
        >
          {alertMessage}
        </Alert>
      )}

      <Row>
        <Col>
          {/* Project Header */}
          <Card className="mb-4">
            <Card.Header className="project-card-header d-flex justify-content-between align-items-center">
              <div>
                <h4 className="project-header-title mb-0">
                  {project.projectName}
                </h4>
                {project.projectDescription && (
                  <p className="project-header-content mt-1">
                    {project.projectDescription}
                  </p>
                )}
              </div>
              <div>
                <Button
                  variant="light"
                  size="sm"
                  className="me-2"
                  onClick={handleEditProject}
                >
                  Edit Project
                </Button>
                <Button
                  variant="outline-light"
                  size="sm"
                  onClick={handleBackToProjects}
                >
                  Back to Projects
                </Button>
              </div>
            </Card.Header>
          </Card>
        </Col>
      </Row>

      <Row>
        {/* Project Users */}
        <Col md={6} className="mb-4">
          <Card>
            <Card.Header>
              <h5 className="mb-0">
                Team Members{" "}
                <Badge bg="secondary">{project.users.length}</Badge>
              </h5>
            </Card.Header>
            <Card.Body>
              {project.users.length === 0 ? (
                <div className="text-center text-muted py-3">
                  <p>No team members assigned to this project.</p>
                </div>
              ) : (
                <ListGroup variant="flush">
                  {project.users.map((user, index) => (
                    <ListGroup.Item key={index}>
                      <div>
                        <div className="fw-bold">{user.userName}</div>
                        <small className="text-muted">{user.userEmail}</small>
                      </div>
                    </ListGroup.Item>
                  ))}
                </ListGroup>
              )}
            </Card.Body>
          </Card>
        </Col>

        {/* Project Tasks */}
        <Col md={6} className="mb-4">
          <Card>
            <Card.Header>
              <h5 className="mb-0">
                Tasks <Badge bg="secondary">{project.tasks.length}</Badge>
              </h5>
            </Card.Header>
            <Card.Body>
              {project.tasks.length === 0 ? (
                <div className="text-center text-muted py-3">
                  <p>No tasks created for this project yet.</p>
                </div>
              ) : (
                <ListGroup variant="flush">
                  {project.tasks.map((task, index) => (
                    <ListGroup.Item key={index}>
                      <div className="fw-bold mb-1">
                        {task.projectTaskTitle}
                      </div>
                      <small className="text-muted">
                        {task.projectTaskDescription || "No description"}
                      </small>
                    </ListGroup.Item>
                  ))}
                </ListGroup>
              )}
            </Card.Body>
          </Card>
        </Col>
      </Row>

      {/* Project Statistics */}
      <Row>
        <Col>
          <Card>
            <Card.Header>
              <h5 className="mb-0">Project Overview</h5>
            </Card.Header>
            <Card.Body>
              <Row className="text-center">
                <Col md={4}>
                  <div className="border-end">
                    <h3 className="text-primary">{project.tasks.length}</h3>
                    <p className="text-muted mb-0">Total Tasks</p>
                  </div>
                </Col>
                <Col md={4}>
                  <div className="border-end">
                    <h3 className="text-success">{project.users.length}</h3>
                    <p className="text-muted mb-0">Team Members</p>
                  </div>
                </Col>
                <Col md={4}>
                  <div>
                    <h3 className="text-info">Active</h3>
                    <p className="text-muted mb-0">Project Status</p>
                  </div>
                </Col>
              </Row>
            </Card.Body>
          </Card>
        </Col>
      </Row>
    </Container>
  );
};

export default ProjectCard;
