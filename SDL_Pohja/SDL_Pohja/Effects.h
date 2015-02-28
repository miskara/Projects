/*Efectit eli shaderit voisi olla erillisessä luokassa. En ole varma onko se välttämätöntä*/

bool init_shaders(){/*
						  int noError = 1;
						  try{
						  GLuint vertShader = glCreateShader(GL_VERTEX_SHADER);
						  if (glGetError())
						  return 0;

						  char *vertex_shader_source = "vertexshader.txt";

						  std::string content;
						  std::ifstream fileStream(vertex_shader_source, std::ios::in);

						  if (!fileStream.is_open()) {
						  std::cerr << "Could not read file " << vertex_shader_source << ". File does not exist." << std::endl;
						  }

						  std::string line = "";
						  while (!fileStream.eof()) {
						  std::getline(fileStream, line);
						  content.append(line + "\n");
						  }

						  fileStream.close();

						  //lisää readfile funktio
						  std::string vertShaderStr = content;
						  const char *vertShaderSrc = vertShaderStr.c_str();

						  glShaderSource(vertShader, 1, &vertShaderSrc, NULL);
						  if (glGetError())
						  return 0;
						  glCompileShader(vertShader);
						  if (glGetError())
						  return 0;

						  GLint program = glCreateProgram();
						  if (glGetError())
						  return 0;
						  glAttachShader(program, vertShader);
						  if (glGetError())
						  return 0;
						  glLinkProgram(program);
						  if (glGetError())
						  return 0;
						  glUseProgram(program);
						  if (glGetError())
						  return 0;
						  myLoc = glGetUniformLocation(program, "z_scale");
						  glUniform1f(myLoc, shaderZ);
						  if (glGetError())
						  return 0;
						  }
						  catch (...){
						  noError = 0;
						  }

						  if (noError == 1){
						  return 1;
						  }
						  else{
						  return 0;
						  }*/

	return false;
}