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
} from "react-bootstrap";
import { useNavigate, useParams } from "react-router-dom";
import axios from "axios";
import { TaskDetailsDTO } from "../../types/types";
import "../projects/project.css";

const TaskView: React.FC = () => {
  const navigate = useNavigate();
  const { projectId, taskId } = useParams<{
    projectId: string;
    taskId: string;
  }>();

  const [task, setTask] = useState<TaskDetailsDTO | null>(null);
  const [loading, setLoading] = useState(true);
  const [showAlert, setShowAlert] = useState(false);
  const [alertMessage, setAlertMessage] = useState("");
  const [alertVariant, setAlertVariant] = useState<"success" | "danger">(
    "success"
  );

  useEffect(() => {
    if (projectId && taskId) {
      fetchTask(projectId, taskId);
    }
  }, [projectId, taskId]);

  const fetchTask = async (projectId: string, taskId: string) => {
    try {
      setLoading(true);
      const response = await axios.get(
        `https://localhost:7272/api/Tasks/project/${projectId}/task/${taskId}`
      );
      setTask(response.data);
    } catch (error: any) {
      console.error("Error fetching task:", error);
      setAlertMessage("Failed to load task details. Please try again.");
      setAlertVariant("danger");
      setShowAlert(true);
      setTimeout(() => setShowAlert(false), 5000);
    } finally {
      setLoading(false);
    }
  };

  const handleBackToProject = () => {
    navigate(`/projects/${projectId}`);
  };

  const handleEditTask = () => {
    navigate(`/projects/${projectId}/tasks/${taskId}/edit`);
  };

  if (loading) {
    return (
      <Container
        className="d-flex justify-content-center align-items-center"
        style={{ minHeight: "100vh" }}
      >
        <div className="text-center">
          <Spinner animation="border" variant="primary" />
          <p className="mt-3">Loading task details...</p>
        </div>
      </Container>
    );
  }

  if (!task) {
    return (
      <Container className="mt-4">
        <Alert variant="danger">
          <Alert.Heading>Task Not Found</Alert.Heading>
          <p>The task you're looking for could not be found.</p>
          <Button variant="primary" onClick={handleBackToProject}>
            Back to Project
          </Button>
        </Alert>
      </Container>
    );
  }

  return (
    <Container
      className="d-flex align-items-center justify-content-center"
      style={{ minHeight: "100vh" }}
    >
      <Row className="w-100">
        <Col xs={12} sm={10} md={8} lg={6} className="mx-auto">
          <Card className="custom-project-card border-0">
            <Card.Header className="custom-project-card-header">
              <Row className="align-items-center">
                <Col xs={12} md={8}>
                  <h4 className="project-card-header-title mb-0">
                    Task Details
                  </h4>
                  <p className="project-header-content mt-1 mb-0">
                    View task information
                  </p>
                </Col>
                <Col
                  xs={12}
                  md={4}
                  className="text-start text-md-end mt-2 mt-md-0"
                >
                  <Button
                    size="sm"
                    className="btn-edit-project me-2"
                    onClick={handleEditTask}
                  >
                    Edit Task
                  </Button>
                  <Button
                    size="sm"
                    className="btn-back-projects"
                    onClick={handleBackToProject}
                  >
                    Back
                  </Button>
                </Col>
              </Row>
            </Card.Header>
            <Card.Body className="p-4">
              {showAlert && (
                <Alert
                  variant={alertVariant}
                  dismissible
                  onClose={() => setShowAlert(false)}
                >
                  {alertMessage}
                </Alert>
              )}

              <div className="mb-4">
                <div className="mb-3">
                  <label className="project-form-label mb-2">Task Title</label>
                  <div className="p-3 bg-light rounded">
                    <h5 className="project-task-name mb-0">
                      {task.projectTaskTitle}
                    </h5>
                  </div>
                </div>

                <div className="mb-3">
                  <label className="project-form-label mb-2">
                    Task Description
                  </label>
                  <div
                    className="p-3 bg-light rounded"
                    style={{ minHeight: "120px" }}
                  >
                    {task.projectTaskDescription ? (
                      <p className="project-task-description mb-0">
                        {task.projectTaskDescription}
                      </p>
                    ) : (
                      <p className="text-muted mb-0 fst-italic">
                        No description provided
                      </p>
                    )}
                  </div>
                </div>

                <div className="mb-3">
                  <label className="project-form-label mb-2">Task ID</label>
                  <div className="p-2 bg-light rounded">
                    <Badge bg="secondary" className="fs-6">
                      {task.id}
                    </Badge>
                  </div>
                </div>
              </div>

              <div className="d-grid gap-2">
                <Button
                  className="btn-edit-project"
                  size="lg"
                  onClick={handleEditTask}
                >
                  Edit Task
                </Button>
                <Button
                  className="btn-cancel"
                  size="lg"
                  onClick={handleBackToProject}
                >
                  Back to Project
                </Button>
              </div>
            </Card.Body>
          </Card>
        </Col>
      </Row>
    </Container>
  );
};

export default TaskView;
