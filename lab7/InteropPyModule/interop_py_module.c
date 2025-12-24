#define PY_SSIZE_T_CLEAN
#include <Python.h>
#include <stddef.h> /* for offsetof() */

typedef struct
  {
  PyObject_HEAD
  PyObject *first; /* first name */
  PyObject *last;  /* last name */
  int age;         /* age */
  double* list;
  Py_ssize_t list_size;
  } InteropPyObject;

static void
InteropPy_dealloc(PyObject *op)
  {
  InteropPyObject *self = (InteropPyObject *) op;
  Py_XDECREF(self->first);
  Py_XDECREF(self->last);
  if (self->list != NULL)
    {
    free(self->list);
    self->list = NULL;
    }
  Py_TYPE(self)->tp_free(self);
  }

static PyObject *
InteropPy_new(PyTypeObject *type, PyObject *args, PyObject *kwds)
  {
  InteropPyObject *self;
  self = (InteropPyObject *) type->tp_alloc(type, 0);
  if (self != NULL) {
    self->first = PyUnicode_FromString("");
    if (self->first == NULL) {
      Py_DECREF(self);
      return NULL;
      }
    self->last = PyUnicode_FromString("");
    if (self->last == NULL) {
      Py_XDECREF(self->first);
      Py_DECREF(self);
      return NULL;
      }
    self->age = 0;
    self->list = NULL;
    self->list_size = 0;
    }
  return (PyObject *) self;
  }

static int
InteropPy_init(PyObject *op, PyObject *args, PyObject *kwds)
  {
  InteropPyObject *self = (InteropPyObject *) op;
  static char *kwlist[] = {"list", "first", "last", "age", NULL};
  PyObject *list = NULL, *first = NULL, *last = NULL;

  if (!PyArg_ParseTupleAndKeywords(args, kwds, "O|OOi", kwlist,
                                   &list, &first, &last,
                                   &self->age))
    return -1;

  if (!PyList_Check(list))
    {
    PyErr_SetString(PyExc_TypeError, "list argument must be a list");
    return -1;
    }

  size_t len = PyList_Size(list);
  self->list = (double*)malloc(len * sizeof(double));
  if (self->list == NULL)
    {
    PyErr_SetString(PyExc_MemoryError, "Unable to allocate memory for list");
    return -1;
    }
  self->list_size = len;

  for (size_t i = 0; i < len; i++)
    {
    PyObject* py_item = PyList_GetItem(list, i);
    double item = 0;
    if (PyFloat_Check(py_item))
      item = PyFloat_AsDouble(py_item);
    else if (PyLong_Check(py_item))
      item = PyLong_AsDouble(py_item);
    else
      {
      free(self->list);
      self->list = NULL;
      self->list_size = 0;
      PyErr_SetString(PyExc_TypeError, "list items must be float");
      return -1;
      }
    self->list[i] = item;
    }

  if (first) {
    Py_XSETREF(self->first, Py_NewRef(first));
    }
  if (last) {
    Py_XSETREF(self->last, Py_NewRef(last));
    }

  return 0;
  }

static PyMemberDef InteropPy_members[] = 
  {
  // first
    { "first", Py_T_OBJECT_EX,  offsetof(InteropPyObject, first), 
    0, "first name" },
  // last
    { "last", Py_T_OBJECT_EX, offsetof(InteropPyObject, last), 
    0, "last name" },
  // age
    { "age", Py_T_INT, offsetof(InteropPyObject, age), 
    0, "some age" },
    {NULL}  /* Sentinel */
  };

static PyObject *
InteropPy_Name(PyObject *op, PyObject *Py_UNUSED(dummy))
  {
  InteropPyObject *self = (InteropPyObject *) op;
  if (self->first == NULL) {
    PyErr_SetString(PyExc_AttributeError, "first");
    return NULL;
    }
  if (self->last == NULL) {
    PyErr_SetString(PyExc_AttributeError, "last");
    return NULL;
    }
  return PyUnicode_FromFormat("%S %S", self->first, self->last);
  }

PyObject* InteropPy_Greet(PyObject *op, PyObject *args)
  {
  InteropPyObject *self = (InteropPyObject *) op;
  char* s;
  if (!PyArg_ParseTuple(args, "s", &s))
    return NULL;
  PyObject* name = PyObject_CallMethod(op, "Name", NULL);
  if (name == NULL)
    printf("Hello, %s!\n", s);
  else
    {
    const char* name_str = PyUnicode_AsUTF8(name);
    printf("%s greets %s!\n", name_str, s);
    Py_DECREF(name);
    }

  Py_RETURN_NONE; //  return None;
  }

PyObject* InteropPy_Mean(PyObject *op, PyObject *args)
  {
  InteropPyObject *self = (InteropPyObject *) op;

  if (self->list == NULL || self->list_size == 0)
    {
    PyErr_SetString(PyExc_ValueError, "list is empty");
    return NULL;
    }

  double sum = 0;
  for (Py_ssize_t i = 0; i < self->list_size; i++)
    {
    sum += self->list[i];
    }

  return PyFloat_FromDouble(sum / self->list_size);
  }

PyObject* InteropPy_RMS(PyObject *op, PyObject *args)
  {
  PyErr_SetString(PyExc_NotImplementedError, "RMS should be implemented in Python addon");
  return NULL;
  }

static PyMethodDef InteropPy_methods[] =
  {
    {
    "Name", InteropPy_Name, METH_NOARGS,
    "Return the name, combining the first and last name"
    },
    {
    "Greet", InteropPy_Greet, METH_VARARGS,
    "Greet someone"
    },
    {
    "Mean", InteropPy_Mean, METH_NOARGS,
    "Calculate mean value"
    },
    {
    "RMS", InteropPy_RMS, METH_NOARGS,
    "Calculate root mean square value"
    },
    {NULL}  /* Sentinel */
  };

static Py_ssize_t
InteropPy_len(InteropPyObject *self)
  {
  return self->list_size;
  }

static PyObject *
InteropPy_getitem(InteropPyObject *self, Py_ssize_t i)
  {
  if (self->list == NULL)
    {
    PyErr_SetString(PyExc_IndexError, "list is empty");
    return NULL;
    }
  if (i < 0)
    i += self->list_size;
  if (i >= self->list_size)
    {
    PyErr_SetString(PyExc_IndexError, "index out of range");
    return NULL;
    }

  return PyFloat_FromDouble(self->list[i]);
  }

static int
InteropPy_setitem(InteropPyObject *self, Py_ssize_t i, PyObject *value)
  {
  if (self->list == NULL)
    {
    PyErr_SetString(PyExc_IndexError, "list is empty");
    return -1;
    }
  if (i < 0)
    i += self->list_size;
  if (i >= self->list_size)
    {
    PyErr_SetString(PyExc_IndexError, "index out of range");
    return -1;
    }

  double item = 0;
  if (PyFloat_Check(value))
    item = PyFloat_AsDouble(value);
  else if (PyLong_Check(value))
    item = PyLong_AsDouble(value);
  else
    {
    PyErr_SetString(PyExc_TypeError, "list items must be float");
    return -1;
    }

  self->list[i] = item;
  return 0;
  }

static PySequenceMethods InteropPy_as_sequence =
  {
    .sq_length = (lenfunc)InteropPy_len,
    .sq_item = (ssizeargfunc)InteropPy_getitem,
    .sq_ass_item = (ssizeobjargproc)InteropPy_setitem,
  };

static PyObject *InteropPy_add(PyObject* pya, PyObject* pyb);
static PyObject* InteropPy_power(PyObject* pya, PyObject* pyb, PyObject* mod);
// pya ** pyb, pya is InteropPy, pyb is PyFloat or PyLong, mod == Py_None
// use pow() from math.h

static PyNumberMethods InteropPy_as_number =
  {
  .nb_add = (binaryfunc)InteropPy_add,
  .nb_power = (ternaryfunc)InteropPy_power,
  };

static PyTypeObject InteropPyType =
  {
  .ob_base = PyVarObject_HEAD_INIT(NULL, 0)
  .tp_name = "interop_lab.InteropPy",
  .tp_doc = PyDoc_STR("InteropPy objects"),
  .tp_basicsize = sizeof(InteropPyObject),
  .tp_itemsize = 0,
  .tp_flags = Py_TPFLAGS_DEFAULT | Py_TPFLAGS_BASETYPE,
  .tp_new = InteropPy_new,
  .tp_init = InteropPy_init,
  .tp_dealloc = InteropPy_dealloc,
  .tp_members = InteropPy_members,
  .tp_methods = InteropPy_methods,
  .tp_as_sequence = &InteropPy_as_sequence,
  .tp_as_number = &InteropPy_as_number,
  };

static PyObject *
InteropPy_add(PyObject* pya, PyObject* pyb)
  {
  if (!PyObject_TypeCheck(pya, &InteropPyType) || !PyObject_TypeCheck(pyb, &InteropPyType))
    {
    PyErr_SetString(PyExc_TypeError, "list items must be float");
    return NULL;
    }

  InteropPyObject* a = (InteropPyObject*)pya;
  InteropPyObject* b = (InteropPyObject*)pyb;

  if (a->list_size != b->list_size)
    {
    PyErr_SetString(PyExc_TypeError, "lists must be of the same size");
    return NULL;
    }

  InteropPyObject* res = (InteropPyObject*)InteropPyType.tp_alloc(&InteropPyType, 0);
  if (res == NULL)
    return NULL;

  res->first = PyUnicode_FromString("");
  if (res->first == NULL)
    {
    Py_DECREF(res);
    return NULL;
    }
  res->last = PyUnicode_FromString("");
  if (res->last == NULL)
    {
    Py_XDECREF(res->first);
    Py_DECREF(res);
    return NULL;
    }
  res->age = 0;
  res->list = NULL;
  res->list_size = a->list_size;

  if (res->list_size == 0)
    return (PyObject *)res;

  res->list = malloc(sizeof(double) * res->list_size);
  if (res->list == NULL)
    {
    Py_XDECREF(res->first);
    Py_XDECREF(res->last);
    Py_DECREF(res);
    PyErr_SetString(PyExc_MemoryError, "Unable to allocate memory for list");
    return NULL;
    }

  for (Py_ssize_t i = 0; i < res->list_size; i++)
    res->list[i] = a->list[i] + b->list[i];

  return (PyObject *)res;
  }

static PyObject* InteropPy_power(PyObject* pya, PyObject* pyb, PyObject* mod)
  {
  if (!PyObject_TypeCheck(pya, &InteropPyType))
    {
    PyErr_SetString(PyExc_TypeError, "list items must be float");
    return NULL;
    }

  double p = 0;

  if (PyFloat_Check(pyb)) {
    p = PyFloat_AsDouble(pyb);
    }
  else if (PyLong_Check(pyb)) {
    p = PyLong_AsDouble(pyb);
    }
  else {
    PyErr_SetString(PyExc_TypeError, "second argument must be numeric");
    return NULL;
    }

  if (Py_None != mod) {
    PyErr_SetString(PyExc_TypeError, "third argument must be none");
    return NULL;
    }

  InteropPyObject* a = (InteropPyObject*)pya;

  InteropPyObject* res = (InteropPyObject*)InteropPyType.tp_alloc(&InteropPyType, 0);
  if (res == NULL)
    return NULL;

  res->first = PyUnicode_FromString("");
  if (res->first == NULL)
    {
    Py_DECREF(res);
    return NULL;
    }
  res->last = PyUnicode_FromString("");
  if (res->last == NULL)
    {
    Py_XDECREF(res->first);
    Py_DECREF(res);
    return NULL;
    }
  res->age = 0;
  res->list = NULL;
  res->list_size = a->list_size;

  if (res->list_size == 0)
    return (PyObject*)res;

  res->list = malloc(sizeof(double) * res->list_size);
  if (res->list == NULL)
    {
    Py_XDECREF(res->first);
    Py_XDECREF(res->last);
    Py_DECREF(res);
    PyErr_SetString(PyExc_MemoryError, "Unable to allocate memory for list");
    return NULL;
    }

  for (Py_ssize_t i = 0; i < res->list_size; i++)
    res->list[i] = pow(a->list[i], p);

  return (PyObject*)res;
  }

static int
interop_lab_module_exec(PyObject *m)
  {
  if (PyType_Ready(&InteropPyType) < 0) {
    return -1;
    }

  if (PyModule_AddObjectRef(m, "InteropPy", (PyObject *) &InteropPyType) < 0) {
    return -1;
    }

  return 0;
  }

static PyModuleDef_Slot interop_lab_module_slots[] =
  {
    {Py_mod_exec, interop_lab_module_exec},
    {Py_mod_multiple_interpreters, Py_MOD_MULTIPLE_INTERPRETERS_NOT_SUPPORTED},
    {0, NULL}
  };

static PyModuleDef interop_lab_module =
  {
  .m_base = PyModuleDef_HEAD_INIT,
  .m_name = "interop_lab",
  .m_doc = "Interop example module.",
  .m_size = 0,
  .m_slots = interop_lab_module_slots,
  };

PyMODINIT_FUNC PyInit_interop_lab(void)
  {
  return PyModuleDef_Init(&interop_lab_module);
  }
