﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CourseProjectTimetable
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class TimetableCourseProject : DbContext
    {
        public TimetableCourseProject()
            : base("name=TimetableCourseProject")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Audience> Audience { get; set; }
        public virtual DbSet<DayNumbers> DayNumbers { get; set; }
        public virtual DbSet<Faculties> Faculties { get; set; }
        public virtual DbSet<Groups> Groups { get; set; }
        public virtual DbSet<PairsNumber> PairsNumber { get; set; }
        public virtual DbSet<PairTypes> PairTypes { get; set; }
        public virtual DbSet<Pulpits> Pulpits { get; set; }
        public virtual DbSet<Specialities> Specialities { get; set; }
        public virtual DbSet<Subjects> Subjects { get; set; }
        public virtual DbSet<Teachers> Teachers { get; set; }
        public virtual DbSet<Timetable> Timetable { get; set; }
    
        public virtual ObjectResult<string> DeleteAudience(string number)
        {
            var numberParameter = number != null ?
                new ObjectParameter("Number", number) :
                new ObjectParameter("Number", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("DeleteAudience", numberParameter);
        }
    
        public virtual ObjectResult<string> DeleteFaculty(string shortName)
        {
            var shortNameParameter = shortName != null ?
                new ObjectParameter("ShortName", shortName) :
                new ObjectParameter("ShortName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("DeleteFaculty", shortNameParameter);
        }
    
        public virtual ObjectResult<string> DeleteGroup(string id)
        {
            var idParameter = id != null ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("DeleteGroup", idParameter);
        }
    
        public virtual ObjectResult<string> DeletePulpit(string shortName)
        {
            var shortNameParameter = shortName != null ?
                new ObjectParameter("ShortName", shortName) :
                new ObjectParameter("ShortName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("DeletePulpit", shortNameParameter);
        }
    
        public virtual ObjectResult<string> DeleteSpeciality(string code)
        {
            var codeParameter = code != null ?
                new ObjectParameter("Code", code) :
                new ObjectParameter("Code", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("DeleteSpeciality", codeParameter);
        }
    
        public virtual ObjectResult<string> DeleteSubjects(string shortName)
        {
            var shortNameParameter = shortName != null ?
                new ObjectParameter("ShortName", shortName) :
                new ObjectParameter("ShortName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("DeleteSubjects", shortNameParameter);
        }
    
        public virtual ObjectResult<string> DeleteTeachers(string id)
        {
            var idParameter = id != null ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("DeleteTeachers", idParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> DeleteTiemtable(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("DeleteTiemtable", idParameter);
        }
    
        public virtual int GroupsFromXML()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GroupsFromXML");
        }
    
        public virtual ObjectResult<string> GroupsToXML()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("GroupsToXML");
        }
    
        public virtual int InsertAudience(string number, Nullable<int> capacity)
        {
            var numberParameter = number != null ?
                new ObjectParameter("number", number) :
                new ObjectParameter("number", typeof(string));
    
            var capacityParameter = capacity.HasValue ?
                new ObjectParameter("capacity", capacity) :
                new ObjectParameter("capacity", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertAudience", numberParameter, capacityParameter);
        }
    
        public virtual int InsertFaculties(string shortName, string fullName)
        {
            var shortNameParameter = shortName != null ?
                new ObjectParameter("ShortName", shortName) :
                new ObjectParameter("ShortName", typeof(string));
    
            var fullNameParameter = fullName != null ?
                new ObjectParameter("FullName", fullName) :
                new ObjectParameter("FullName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertFaculties", shortNameParameter, fullNameParameter);
        }
    
        public virtual int InsertGroups(string id, string specialityCode, Nullable<int> course, Nullable<int> number, Nullable<int> count)
        {
            var idParameter = id != null ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(string));
    
            var specialityCodeParameter = specialityCode != null ?
                new ObjectParameter("SpecialityCode", specialityCode) :
                new ObjectParameter("SpecialityCode", typeof(string));
    
            var courseParameter = course.HasValue ?
                new ObjectParameter("Course", course) :
                new ObjectParameter("Course", typeof(int));
    
            var numberParameter = number.HasValue ?
                new ObjectParameter("Number", number) :
                new ObjectParameter("Number", typeof(int));
    
            var countParameter = count.HasValue ?
                new ObjectParameter("Count", count) :
                new ObjectParameter("Count", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertGroups", idParameter, specialityCodeParameter, courseParameter, numberParameter, countParameter);
        }
    
        public virtual int InsertPulpit(string shortName, string fullName, string shortFacultyName)
        {
            var shortNameParameter = shortName != null ?
                new ObjectParameter("ShortName", shortName) :
                new ObjectParameter("ShortName", typeof(string));
    
            var fullNameParameter = fullName != null ?
                new ObjectParameter("FullName", fullName) :
                new ObjectParameter("FullName", typeof(string));
    
            var shortFacultyNameParameter = shortFacultyName != null ?
                new ObjectParameter("ShortFacultyName", shortFacultyName) :
                new ObjectParameter("ShortFacultyName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertPulpit", shortNameParameter, fullNameParameter, shortFacultyNameParameter);
        }
    
        public virtual int InsertSpeciality(string code, string shortFacultyName, string shortName, string fullName, string qualification)
        {
            var codeParameter = code != null ?
                new ObjectParameter("Code", code) :
                new ObjectParameter("Code", typeof(string));
    
            var shortFacultyNameParameter = shortFacultyName != null ?
                new ObjectParameter("ShortFacultyName", shortFacultyName) :
                new ObjectParameter("ShortFacultyName", typeof(string));
    
            var shortNameParameter = shortName != null ?
                new ObjectParameter("ShortName", shortName) :
                new ObjectParameter("ShortName", typeof(string));
    
            var fullNameParameter = fullName != null ?
                new ObjectParameter("FullName", fullName) :
                new ObjectParameter("FullName", typeof(string));
    
            var qualificationParameter = qualification != null ?
                new ObjectParameter("Qualification", qualification) :
                new ObjectParameter("Qualification", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertSpeciality", codeParameter, shortFacultyNameParameter, shortNameParameter, fullNameParameter, qualificationParameter);
        }
    
        public virtual int InsertSubjects(string shortName, string fullName)
        {
            var shortNameParameter = shortName != null ?
                new ObjectParameter("ShortName", shortName) :
                new ObjectParameter("ShortName", typeof(string));
    
            var fullNameParameter = fullName != null ?
                new ObjectParameter("FullName", fullName) :
                new ObjectParameter("FullName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertSubjects", shortNameParameter, fullNameParameter);
        }
    
        public virtual int InsertTeachers(string id, string name, string surname, string patronymic, string shortPulpitName)
        {
            var idParameter = id != null ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(string));
    
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            var surnameParameter = surname != null ?
                new ObjectParameter("Surname", surname) :
                new ObjectParameter("Surname", typeof(string));
    
            var patronymicParameter = patronymic != null ?
                new ObjectParameter("Patronymic", patronymic) :
                new ObjectParameter("Patronymic", typeof(string));
    
            var shortPulpitNameParameter = shortPulpitName != null ?
                new ObjectParameter("ShortPulpitName", shortPulpitName) :
                new ObjectParameter("ShortPulpitName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertTeachers", idParameter, nameParameter, surnameParameter, patronymicParameter, shortPulpitNameParameter);
        }
    
        public virtual int InsertTimeTable(Nullable<int> dayNumberId, Nullable<int> pairNumber, string weekNumber, string groupId, string subgroup, string shortSubjectName, string audienceNumber, string teacherId, string shortPairtypeName)
        {
            var dayNumberIdParameter = dayNumberId.HasValue ?
                new ObjectParameter("DayNumberId", dayNumberId) :
                new ObjectParameter("DayNumberId", typeof(int));
    
            var pairNumberParameter = pairNumber.HasValue ?
                new ObjectParameter("PairNumber", pairNumber) :
                new ObjectParameter("PairNumber", typeof(int));
    
            var weekNumberParameter = weekNumber != null ?
                new ObjectParameter("WeekNumber", weekNumber) :
                new ObjectParameter("WeekNumber", typeof(string));
    
            var groupIdParameter = groupId != null ?
                new ObjectParameter("GroupId", groupId) :
                new ObjectParameter("GroupId", typeof(string));
    
            var subgroupParameter = subgroup != null ?
                new ObjectParameter("Subgroup", subgroup) :
                new ObjectParameter("Subgroup", typeof(string));
    
            var shortSubjectNameParameter = shortSubjectName != null ?
                new ObjectParameter("ShortSubjectName", shortSubjectName) :
                new ObjectParameter("ShortSubjectName", typeof(string));
    
            var audienceNumberParameter = audienceNumber != null ?
                new ObjectParameter("AudienceNumber", audienceNumber) :
                new ObjectParameter("AudienceNumber", typeof(string));
    
            var teacherIdParameter = teacherId != null ?
                new ObjectParameter("TeacherId", teacherId) :
                new ObjectParameter("TeacherId", typeof(string));
    
            var shortPairtypeNameParameter = shortPairtypeName != null ?
                new ObjectParameter("ShortPairtypeName", shortPairtypeName) :
                new ObjectParameter("ShortPairtypeName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertTimeTable", dayNumberIdParameter, pairNumberParameter, weekNumberParameter, groupIdParameter, subgroupParameter, shortSubjectNameParameter, audienceNumberParameter, teacherIdParameter, shortPairtypeNameParameter);
        }
    
        public virtual ObjectResult<string> UpdateAudience(string number, Nullable<int> newCapacity)
        {
            var numberParameter = number != null ?
                new ObjectParameter("Number", number) :
                new ObjectParameter("Number", typeof(string));
    
            var newCapacityParameter = newCapacity.HasValue ?
                new ObjectParameter("NewCapacity", newCapacity) :
                new ObjectParameter("NewCapacity", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("UpdateAudience", numberParameter, newCapacityParameter);
        }
    
        public virtual ObjectResult<string> UpdateFaculty(string shortName, string newFullName)
        {
            var shortNameParameter = shortName != null ?
                new ObjectParameter("ShortName", shortName) :
                new ObjectParameter("ShortName", typeof(string));
    
            var newFullNameParameter = newFullName != null ?
                new ObjectParameter("NewFullName", newFullName) :
                new ObjectParameter("NewFullName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("UpdateFaculty", shortNameParameter, newFullNameParameter);
        }
    
        public virtual ObjectResult<string> UpdateGroup(string id, Nullable<int> newCount)
        {
            var idParameter = id != null ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(string));
    
            var newCountParameter = newCount.HasValue ?
                new ObjectParameter("NewCount", newCount) :
                new ObjectParameter("NewCount", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("UpdateGroup", idParameter, newCountParameter);
        }
    
        public virtual ObjectResult<string> UpdatePulpit(string shortName, string newFullName, string newShortFacultyName)
        {
            var shortNameParameter = shortName != null ?
                new ObjectParameter("ShortName", shortName) :
                new ObjectParameter("ShortName", typeof(string));
    
            var newFullNameParameter = newFullName != null ?
                new ObjectParameter("NewFullName", newFullName) :
                new ObjectParameter("NewFullName", typeof(string));
    
            var newShortFacultyNameParameter = newShortFacultyName != null ?
                new ObjectParameter("NewShortFacultyName", newShortFacultyName) :
                new ObjectParameter("NewShortFacultyName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("UpdatePulpit", shortNameParameter, newFullNameParameter, newShortFacultyNameParameter);
        }
    
        public virtual ObjectResult<string> UpdateSpeciality(string code, string newShortFacultyName, string newShortName, string newFullName, string newQualification)
        {
            var codeParameter = code != null ?
                new ObjectParameter("Code", code) :
                new ObjectParameter("Code", typeof(string));
    
            var newShortFacultyNameParameter = newShortFacultyName != null ?
                new ObjectParameter("NewShortFacultyName", newShortFacultyName) :
                new ObjectParameter("NewShortFacultyName", typeof(string));
    
            var newShortNameParameter = newShortName != null ?
                new ObjectParameter("NewShortName", newShortName) :
                new ObjectParameter("NewShortName", typeof(string));
    
            var newFullNameParameter = newFullName != null ?
                new ObjectParameter("NewFullName", newFullName) :
                new ObjectParameter("NewFullName", typeof(string));
    
            var newQualificationParameter = newQualification != null ?
                new ObjectParameter("NewQualification", newQualification) :
                new ObjectParameter("NewQualification", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("UpdateSpeciality", codeParameter, newShortFacultyNameParameter, newShortNameParameter, newFullNameParameter, newQualificationParameter);
        }
    
        public virtual ObjectResult<string> UpdateSubjects(string shortName, string newFullName)
        {
            var shortNameParameter = shortName != null ?
                new ObjectParameter("ShortName", shortName) :
                new ObjectParameter("ShortName", typeof(string));
    
            var newFullNameParameter = newFullName != null ?
                new ObjectParameter("NewFullName", newFullName) :
                new ObjectParameter("NewFullName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("UpdateSubjects", shortNameParameter, newFullNameParameter);
        }
    
        public virtual ObjectResult<string> UpdateTeachers(string id, string newShortPulpitName)
        {
            var idParameter = id != null ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(string));
    
            var newShortPulpitNameParameter = newShortPulpitName != null ?
                new ObjectParameter("NewShortPulpitName", newShortPulpitName) :
                new ObjectParameter("NewShortPulpitName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("UpdateTeachers", idParameter, newShortPulpitNameParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> UpdateTiemtable(Nullable<int> id, Nullable<int> newDayNumberId, Nullable<int> newPairNumber, string newWeekNumber, string newGroupId, string newSubgroup, string newShortSubjectName, string newAudienceNumber, string newTeacherId, string newShortPairtypeName)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            var newDayNumberIdParameter = newDayNumberId.HasValue ?
                new ObjectParameter("NewDayNumberId", newDayNumberId) :
                new ObjectParameter("NewDayNumberId", typeof(int));
    
            var newPairNumberParameter = newPairNumber.HasValue ?
                new ObjectParameter("NewPairNumber", newPairNumber) :
                new ObjectParameter("NewPairNumber", typeof(int));
    
            var newWeekNumberParameter = newWeekNumber != null ?
                new ObjectParameter("NewWeekNumber", newWeekNumber) :
                new ObjectParameter("NewWeekNumber", typeof(string));
    
            var newGroupIdParameter = newGroupId != null ?
                new ObjectParameter("NewGroupId", newGroupId) :
                new ObjectParameter("NewGroupId", typeof(string));
    
            var newSubgroupParameter = newSubgroup != null ?
                new ObjectParameter("NewSubgroup", newSubgroup) :
                new ObjectParameter("NewSubgroup", typeof(string));
    
            var newShortSubjectNameParameter = newShortSubjectName != null ?
                new ObjectParameter("NewShortSubjectName", newShortSubjectName) :
                new ObjectParameter("NewShortSubjectName", typeof(string));
    
            var newAudienceNumberParameter = newAudienceNumber != null ?
                new ObjectParameter("NewAudienceNumber", newAudienceNumber) :
                new ObjectParameter("NewAudienceNumber", typeof(string));
    
            var newTeacherIdParameter = newTeacherId != null ?
                new ObjectParameter("NewTeacherId", newTeacherId) :
                new ObjectParameter("NewTeacherId", typeof(string));
    
            var newShortPairtypeNameParameter = newShortPairtypeName != null ?
                new ObjectParameter("NewShortPairtypeName", newShortPairtypeName) :
                new ObjectParameter("NewShortPairtypeName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("UpdateTiemtable", idParameter, newDayNumberIdParameter, newPairNumberParameter, newWeekNumberParameter, newGroupIdParameter, newSubgroupParameter, newShortSubjectNameParameter, newAudienceNumberParameter, newTeacherIdParameter, newShortPairtypeNameParameter);
        }
    }
}
