import React, { useState, useEffect } from "react";
import {
  Container,
  Row,
  Col,
  Card,
  Form,
  Button,
  Spinner,
} from "react-bootstrap";
import { useNavigate, useParams } from "react-router-dom";
import { Formik } from "formik";
import * as Yup from "yup";
import axios from "axios";
import { SingleTaskDTO } from "../../types/types";
import Loader from "../../components/common/Loader";
import Alert from "../../components/common/Alert";
import "../projects/project.css";

interface TaskFormData {
  projectTaskTitle: string;
  projectTaskDescription: string;
}

// Validation schema using Yup
const validationSchema = Yup.object({
  projectTaskTitle: Yup.string()
    .min(3, "Task title must be at least 3 characters")
    .max(200, "Task title must be less than 200 characters")
    .required("Task title is required"),
  projectTaskDescription: Yup.string().max(
    1000,
    "Description must be less than 1000 characters"
  ),
});

const TaskForm: React.FC = () => {
  const navigate = useNavigate();
  const { projectId, taskId } = useParams<{
    projectId: string;
    taskId: string;
  }>();
  const isEditing = Boolean(taskId);

  const [showAlert, setShowAlert] = useState(false);
  const [alertMessage, setAlertMessage] = useState("");
  const [alertVariant, setAlertVariant] = useState<"success" | "danger">(
    "success"
  );
  const [initialLoading, setInitialLoading] = useState(isEditing);

  const initialValues: TaskFormData = {
    projectTaskTitle: "",
    projectTaskDescription: "",
  };

  const [formValues, setFormValues] = useState<TaskFormData>(initialValues);

  useEffect(() => {
    if (isEditing && projectId && taskId) {
      fetchTask(projectId, taskId);
    }
  }, [isEditing, projectId, taskId]);

  const fetchTask = async (projectId: string, taskId: string) => {
    try {
      setInitialLoading(true);
      const response = await axios.get(
        `https://localhost:7272/api/Tasks/project/${projectId}/task/${taskId}`
      );
      const task: SingleTaskDTO = response.data;
      setFormValues({
        projectTaskTitle: task.projectTaskTitle,
        projectTaskDescription: task.projectTaskDescription || "",
      });
    } catch (error: any) {
      console.error("Error fetching task:", error);
      setAlertMessage("Failed to load task. Please try again.");
      setAlertVariant("danger");
      setShowAlert(true);
    } finally {
      setInitialLoading(false);
    }
  };

  const handleSubmit = async (
    values: TaskFormData,
    { setSubmitting, setFieldError }: any
  ) => {
    try {
      if (isEditing && projectId && taskId) {
        // Update existing task
        const response = await axios.put(
          `https://localhost:7272/api/Tasks/project/${projectId}/task/${taskId}`,
          values
        );
        console.log("Task updated:", response.data);
        setAlertMessage("Task updated successfully!");
      } else if (projectId) {
        // Create new task
        const response = await axios.post(
          `https://localhost:7272/api/Tasks/project/${projectId}`,
          values
        );
        console.log("Task created:", response.data);
        setAlertMessage("Task created successfully!");
      }

      setAlertVariant("success");
      setShowAlert(true);

      // Redirect back to project after 2 seconds
      setTimeout(() => {
        navigate(`/projects/${projectId}`);
      }, 2000);
    } catch (error: any) {
      console.error("Task submission error:", error);

      if (error.response?.data?.message) {
        setAlertMessage(error.response.data.message);
      } else if (error.response?.data?.errors) {
        // Handle validation errors from backend
        Object.keys(error.response.data.errors).forEach((field) => {
          setFieldError(
            field.toLowerCase(),
            error.response.data.errors[field][0]
          );
        });
        setAlertMessage("Please fix the errors below");
      } else {
        setAlertMessage(
          isEditing
            ? "Failed to update task. Please try again."
            : "Failed to create task. Please try again."
        );
      }

      setAlertVariant("danger");
      setShowAlert(true);
    } finally {
      setSubmitting(false);
    }
  };

  const handleCancel = () => {
    navigate(`/projects/${projectId}`);
  };

  if (initialLoading) {
    return <Loader message="Loading task..." />;
  }

  return (
    <Container
      className="d-flex align-items-center justify-content-center"
      style={{ minHeight: "100vh" }}
    >
      <Row className="w-100">
        <Col xs={12} sm={10} md={8} lg={6} className="mx-auto">
          <Card className="custom-project-card border-0">
            <Card.Header className="text-center custom-project-card-header py-4">
              <h4 className="project-card-header-title mb-0">
                {isEditing ? "Edit Task" : "Create New Task"}
              </h4>
            </Card.Header>
            <Card.Body className="p-4">
              <Alert
                show={showAlert}
                variant={alertVariant}
                message={alertMessage}
                onClose={() => setShowAlert(false)}
              />

              <Formik
                initialValues={formValues}
                validationSchema={validationSchema}
                enableReinitialize={true}
                onSubmit={handleSubmit}
              >
                {({
                  values,
                  errors,
                  touched,
                  handleChange,
                  handleBlur,
                  handleSubmit,
                  isSubmitting,
                }) => (
                  <Form noValidate onSubmit={handleSubmit}>
                    <Form.Group className="mb-3" controlId="formTaskTitle">
                      <Form.Label className="project-form-label">
                        Task Title *
                      </Form.Label>
                      <Form.Control
                        type="text"
                        name="projectTaskTitle"
                        placeholder="Enter task title"
                        value={values.projectTaskTitle}
                        onChange={handleChange}
                        onBlur={handleBlur}
                        isInvalid={
                          touched.projectTaskTitle && !!errors.projectTaskTitle
                        }
                      />
                      <Form.Control.Feedback type="invalid">
                        {errors.projectTaskTitle}
                      </Form.Control.Feedback>
                    </Form.Group>

                    <Form.Group
                      className="mb-3"
                      controlId="formTaskDescription"
                    >
                      <Form.Label className="project-form-label">
                        Task Description
                      </Form.Label>
                      <Form.Control
                        as="textarea"
                        rows={4}
                        name="projectTaskDescription"
                        placeholder="Enter task description (optional)"
                        value={values.projectTaskDescription}
                        onChange={handleChange}
                        onBlur={handleBlur}
                        isInvalid={
                          touched.projectTaskDescription &&
                          !!errors.projectTaskDescription
                        }
                      />
                      <Form.Control.Feedback type="invalid">
                        {errors.projectTaskDescription}
                      </Form.Control.Feedback>
                      <Form.Text className="project-form-footer">
                        Optional. Maximum 1000 characters.
                      </Form.Text>
                    </Form.Group>

                    <div className="d-grid gap-2">
                      <Button
                        className="btn-create-project"
                        type="submit"
                        size="lg"
                        disabled={isSubmitting}
                      >
                        {isSubmitting ? (
                          <>
                            <Spinner
                              as="span"
                              animation="border"
                              size="sm"
                              role="status"
                              className="me-2"
                            />
                            {isEditing ? "Updating..." : "Creating..."}
                          </>
                        ) : isEditing ? (
                          "Update Task"
                        ) : (
                          "Create Task"
                        )}
                      </Button>
                      <Button
                        className="btn-cancel"
                        size="lg"
                        onClick={handleCancel}
                        disabled={isSubmitting}
                      >
                        Cancel
                      </Button>
                    </div>
                  </Form>
                )}
              </Formik>
            </Card.Body>
          </Card>
        </Col>
      </Row>
    </Container>
  );
};

export default TaskForm;
