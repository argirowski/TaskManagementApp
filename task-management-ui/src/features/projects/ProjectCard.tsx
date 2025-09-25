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
import { ProjectDetailsDTO, TaskDetailsDTO } from "../../types/types";
import ConfirmDialog from "../../components/common/ConfirmDialog";
import "./project.css";

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
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [taskToDelete, setTaskToDelete] = useState<TaskDetailsDTO | null>(null);

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

  const handleDeleteTask = (task: TaskDetailsDTO) => {
    setTaskToDelete(task);
    setShowDeleteModal(true);
  };

  const handleDeleteConfirm = async () => {
    if (!taskToDelete || !id) return;

    try {
      await axios.delete(
        `https://localhost:7272/api/Tasks/project/${id}/task/${taskToDelete.id}`
      );

      setAlertMessage("Task deleted successfully!");
      setAlertVariant("success");
      setShowAlert(true);
      setTimeout(() => setShowAlert(false), 3000);

      // Refresh the project data
      fetchProject(id);
    } catch (error: any) {
      console.error("Error deleting task:", error);
      setAlertMessage("Failed to delete task. Please try again.");
      setAlertVariant("danger");
      setShowAlert(true);
      setTimeout(() => setShowAlert(false), 5000);
    } finally {
      setShowDeleteModal(false);
      setTaskToDelete(null);
    }
  };

  const handleDeleteCancel = () => {
    setShowDeleteModal(false);
    setTaskToDelete(null);
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
            <Card.Header className="project-card-header">
              <Row className="align-items-center">
                <Col xs={12} md={6} lg={9}>
                  <h4 className="project-header-title mb-0">
                    {project.projectName}
                  </h4>
                  {project.projectDescription && (
                    <p className="project-header-content mt-1 mb-0">
                      {project.projectDescription}
                    </p>
                  )}
                </Col>
                <Col xs={12} md={6} lg={3} className="text-md-end mt-2 mt-md-0">
                  <Button
                    size="sm"
                    className="btn-edit-project me-2"
                    onClick={handleEditProject}
                  >
                    Edit Project
                  </Button>
                  <Button
                    size="sm"
                    className="btn-back-projects"
                    onClick={handleBackToProjects}
                  >
                    Back
                  </Button>
                </Col>
              </Row>
            </Card.Header>
          </Card>
        </Col>
      </Row>

      <Row>
        {/* Project Users */}
        <Col md={6} className="mb-4">
          <Card>
            <Card.Header className="project-section-card-header">
              <h5 className="project-section-header">
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
                        <div className="project-member-name">
                          {user.userName}
                        </div>
                        <p className="project-member-email">{user.userEmail}</p>
                      </div>
                    </ListGroup.Item>
                  ))}
                </ListGroup>
              )}
            </Card.Body>
          </Card>
        </Col>

        {/* Project Statistics */}
        <Col md={6} className="mb-4">
          <Card>
            <Card.Header className="project-section-card-header">
              <h5 className="project-section-header">Project Overview</h5>
            </Card.Header>
            <Card.Body>
              <Row className="text-center">
                <Col md={4}>
                  <div className="border-end">
                    <h3 className="project-stat-number">
                      {project.tasks.length}
                    </h3>
                    <p className="project-stat-number-text mb-0">Total Tasks</p>
                  </div>
                </Col>
                <Col md={4}>
                  <div className="border-end">
                    <h3 className="project-stat-number">
                      {project.users.length}
                    </h3>
                    <p className="project-stat-number-text mb-0">
                      Team Members
                    </p>
                  </div>
                </Col>
                <Col md={4}>
                  <div>
                    <h3 className="project-stat-number">Active</h3>
                    <p className="project-stat-number-text mb-0">
                      Project Status
                    </p>
                  </div>
                </Col>
              </Row>
            </Card.Body>
          </Card>
        </Col>
      </Row>

      {/* Project Tasks */}
      <Row>
        <Col md={12} className="mb-4">
          <Card>
            <Card.Header className="project-section-card-header">
              <Row className="align-items-center">
                <Col>
                  <h5 className="project-section-header mb-0">
                    Tasks <Badge bg="secondary">{project.tasks.length}</Badge>
                  </h5>
                </Col>
                <Col xs="auto">
                  <Button
                    size="sm"
                    className="btn-edit-project"
                    onClick={() => navigate(`/projects/${id}/tasks/new`)}
                  >
                    Add Task
                  </Button>
                </Col>
              </Row>
            </Card.Header>
            <Card.Body>
              {project.tasks.length === 0 ? (
                <div className="text-center text-muted py-3">
                  <p>No tasks created for this project yet.</p>
                </div>
              ) : (
                <ListGroup variant="flush">
                  {project.tasks.map((task) => (
                    <ListGroup.Item key={task.id}>
                      <Row className="align-items-center">
                        <Col md={8}>
                          <div className="project-task-name mb-1">
                            {task.projectTaskTitle}
                          </div>
                          <small className="project-task-description">
                            {task.projectTaskDescription || "No description"}
                          </small>
                        </Col>
                        <Col
                          md={4}
                          className="text-start text-md-end mt-2 mt-md-0"
                        >
                          <Button
                            size="sm"
                            className="btn-project-view me-2"
                            onClick={() =>
                              navigate(`/projects/${id}/tasks/${task.id}`)
                            }
                          >
                            View
                          </Button>
                          <Button
                            size="sm"
                            className="btn-task-edit me-2"
                            onClick={() =>
                              navigate(`/projects/${id}/tasks/${task.id}/edit`)
                            }
                          >
                            Edit
                          </Button>
                          <Button
                            size="sm"
                            className="btn-project-delete"
                            onClick={() => handleDeleteTask(task)}
                          >
                            Delete
                          </Button>
                        </Col>
                      </Row>
                    </ListGroup.Item>
                  ))}
                </ListGroup>
              )}
            </Card.Body>
          </Card>
        </Col>
      </Row>

      {/* Task Delete Confirmation Dialog */}
      <ConfirmDialog
        show={showDeleteModal}
        title="Confirm Delete"
        message={`Are you sure you want to delete the task "${taskToDelete?.projectTaskTitle}"?`}
        confirmText="Delete"
        cancelText="Cancel"
        onConfirm={handleDeleteConfirm}
        onCancel={handleDeleteCancel}
        variant="danger"
      />
    </Container>
  );
};

export default ProjectCard;
