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
import { TaskFormData } from "../../types/types";
import { fetchTask, createTask, updateTask } from "../../services/taskService";
import LoaderComponent from "../../components/common/LoaderComponent";
import AlertComponent from "../../components/common/AlertComponent";
import { tasksValidationSchema } from "../../utils/validation";
import ConfirmDialogComponent from "../../components/common/ConfirmDialogComponent";

// Validation schema using Yup

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
  // Modal state for cancel confirmation
  const [showConfirm, setShowConfirm] = useState(false);

  const initialValues: TaskFormData = {
    projectTaskTitle: "",
    projectTaskDescription: "",
  };

  const [formValues, setFormValues] = useState<TaskFormData>(initialValues);

  useEffect(() => {
    if (isEditing && projectId && taskId) {
      loadTask(projectId, taskId);
    }
  }, [isEditing, projectId, taskId]);

  const loadTask = async (projectId: string, taskId: string) => {
    try {
      setInitialLoading(true);
      const task = await fetchTask(projectId, taskId);
      setFormValues({
        projectTaskTitle: task.projectTaskTitle,
        projectTaskDescription: task.projectTaskDescription || "",
      });
    } catch (error: any) {
      console.error("Failed to load task:", error);
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
        const updatedTask = await updateTask(projectId, taskId, values);
        console.log("Task updated:", updatedTask);
        setAlertMessage("Task updated successfully!");
      } else if (projectId) {
        // Create new task
        const newTask = await createTask(projectId, values);
        console.log("Task created:", newTask);
      }

      // Redirect back to project immediately
      navigate(`/projects/${projectId}`);
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

  if (initialLoading) {
    return <LoaderComponent message="Loading task..." />;
  }

  return (
    <Container
      className="d-flex align-items-center justify-content-center"
      style={{ minHeight: "100vh" }}
    >
      <Row className="w-100">
        <Col xs={12} sm={10} md={8} lg={6} className="mx-auto">
          <Card className="custom-task-card border-0">
            <Card.Header className="text-center custom-task-card-header py-4">
              <h4 className="project-task-header-title mb-0">
                {isEditing ? "Edit Task" : "Create New Task"}
              </h4>
            </Card.Header>
            <Card.Body className="p-4">
              <AlertComponent
                show={showAlert}
                variant={alertVariant}
                message={alertMessage}
                onClose={() => setShowAlert(false)}
              />

              <Formik
                initialValues={formValues}
                validationSchema={tasksValidationSchema}
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
                  dirty,
                }) => {
                  // Handler for Back to Home, now inside Formik
                  const handleBackToHome = () => {
                    if (dirty) {
                      setShowConfirm(true);
                    } else {
                      navigate(-1);
                    }
                  };
                  return (
                    <>
                      {" "}
                      <Form noValidate onSubmit={handleSubmit}>
                        <Form.Group className="mb-3" controlId="formTaskTitle">
                          <Form.Label className="task-form-label">
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
                              touched.projectTaskTitle &&
                              !!errors.projectTaskTitle
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
                          <Form.Label className="task-form-label">
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
                          <Form.Text className="task-form-footer">
                            Optional. Maximum 500 characters.
                          </Form.Text>
                        </Form.Group>

                        <div className="d-grid gap-2">
                          <Button
                            className="btn-create-task"
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
                            onClick={handleBackToHome}
                            disabled={isSubmitting}
                          >
                            Cancel
                          </Button>
                        </div>
                      </Form>
                      <ConfirmDialogComponent
                        show={showConfirm}
                        title="Cancel Task?"
                        message="Are you sure you want to cancel? Your changes will be lost."
                        confirmText="Yes"
                        cancelText="No"
                        onConfirm={() => {
                          setShowConfirm(false);
                          navigate(-1);
                        }}
                        onCancel={() => setShowConfirm(false)}
                        variant="danger"
                      />
                    </>
                  );
                }}
              </Formik>
            </Card.Body>
          </Card>
        </Col>
      </Row>
    </Container>
  );
};

export default TaskForm;
