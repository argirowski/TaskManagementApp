import React, { useState, useEffect } from "react";
import { Container, Button, Card, Col, Row } from "react-bootstrap";
import { useNavigate, useParams } from "react-router-dom";
import axios from "axios";
import { SingleTaskDTO } from "../../types/types";
import Loader from "../../components/common/Loader";
import EmptyState from "../../components/common/EmptyState";
import "../projects/project.css";

const TaskView: React.FC = () => {
  const navigate = useNavigate();
  const { projectId, taskId } = useParams<{
    projectId: string;
    taskId: string;
  }>();

  const [task, setTask] = useState<SingleTaskDTO | null>(null);
  const [loading, setLoading] = useState(true);

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
    return <Loader message="Loading task details..." />;
  }

  if (!task) {
    return (
      <Container className="mt-4">
        <EmptyState
          title="Task Not Found"
          message="The task you're looking for could not be found."
          actionText="Back to Project"
          onAction={handleBackToProject}
        />
      </Container>
    );
  }

  return (
    <Container
      className="d-flex align-items-center justify-content-center"
      style={{ minHeight: "100vh" }}
    >
      <Row className="w-100">
        <Col xs={12} sm={10} md={8} lg={6} xl={5} className="mx-auto">
          <Card className="custom-card border-0">
            <Card.Header className="text-center custom-card-header py-4">
              <h4 className="header-title mb-0">Task Details</h4>
            </Card.Header>
            <Card.Body className="p-4">
              <div className="mb-3">
                <h5>Title:</h5>
                <p>{task.projectTaskTitle}</p>
              </div>
              <div className="mb-4">
                <h5>Description:</h5>
                <p>
                  {task.projectTaskDescription || "No description provided"}
                </p>
              </div>
              <div className="d-grid gap-2">
                <Button
                  type="button"
                  size="lg"
                  onClick={handleEditTask}
                  className="btn-create-account"
                >
                  Edit Task
                </Button>
                <Button
                  onClick={handleBackToProject}
                  size="lg"
                  className="btn-back-home"
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
