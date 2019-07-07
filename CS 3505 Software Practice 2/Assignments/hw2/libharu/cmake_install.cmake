# Install script for directory: /home/aaron/Documents/hw2/libharu

# Set the install prefix
if(NOT DEFINED CMAKE_INSTALL_PREFIX)
  set(CMAKE_INSTALL_PREFIX "/usr/local")
endif()
string(REGEX REPLACE "/$" "" CMAKE_INSTALL_PREFIX "${CMAKE_INSTALL_PREFIX}")

# Set the install configuration name.
if(NOT DEFINED CMAKE_INSTALL_CONFIG_NAME)
  if(BUILD_TYPE)
    string(REGEX REPLACE "^[^A-Za-z0-9_]+" ""
           CMAKE_INSTALL_CONFIG_NAME "${BUILD_TYPE}")
  else()
    set(CMAKE_INSTALL_CONFIG_NAME "")
  endif()
  message(STATUS "Install configuration: \"${CMAKE_INSTALL_CONFIG_NAME}\"")
endif()

# Set the component getting installed.
if(NOT CMAKE_INSTALL_COMPONENT)
  if(COMPONENT)
    message(STATUS "Install component: \"${COMPONENT}\"")
    set(CMAKE_INSTALL_COMPONENT "${COMPONENT}")
  else()
    set(CMAKE_INSTALL_COMPONENT)
  endif()
endif()

# Install shared libraries without execute permission?
if(NOT DEFINED CMAKE_INSTALL_SO_NO_EXE)
  set(CMAKE_INSTALL_SO_NO_EXE "1")
endif()

# Is this installation the result of a crosscompile?
if(NOT DEFINED CMAKE_CROSSCOMPILING)
  set(CMAKE_CROSSCOMPILING "FALSE")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include" TYPE FILE FILES
    "/home/aaron/Documents/hw2/libharu/include/hpdf.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_types.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_consts.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_version.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_annotation.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_catalog.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_conf.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_destination.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_doc.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_encoder.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_encrypt.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_encryptdict.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_error.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_ext_gstate.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_font.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_fontdef.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_gstate.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_image.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_info.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_list.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_mmgr.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_objects.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_outline.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_pages.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_page_label.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_streams.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_u3d.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_utils.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_pdfa.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_3dmeasure.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_exdata.h"
    "/home/aaron/Documents/hw2/libharu/include/hpdf_config.h"
    )
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/." TYPE FILE FILES
    "/home/aaron/Documents/hw2/libharu/README"
    "/home/aaron/Documents/hw2/libharu/CHANGES"
    "/home/aaron/Documents/hw2/libharu/INSTALL"
    )
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/." TYPE DIRECTORY FILES "/home/aaron/Documents/hw2/libharu/if")
endif()

if(NOT CMAKE_INSTALL_LOCAL_ONLY)
  # Include the install script for each subdirectory.
  include("/home/aaron/Documents/hw2/libharu/src/cmake_install.cmake")
  include("/home/aaron/Documents/hw2/libharu/demo/cmake_install.cmake")

endif()

if(CMAKE_INSTALL_COMPONENT)
  set(CMAKE_INSTALL_MANIFEST "install_manifest_${CMAKE_INSTALL_COMPONENT}.txt")
else()
  set(CMAKE_INSTALL_MANIFEST "install_manifest.txt")
endif()

string(REPLACE ";" "\n" CMAKE_INSTALL_MANIFEST_CONTENT
       "${CMAKE_INSTALL_MANIFEST_FILES}")
file(WRITE "/home/aaron/Documents/hw2/libharu/${CMAKE_INSTALL_MANIFEST}"
     "${CMAKE_INSTALL_MANIFEST_CONTENT}")
