using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;

namespace xpf.Http.Spec
{
    public class FeatureAttribute : SubjectAttribute
    {
        public FeatureAttribute(Type subjectType) : base(subjectType)
        {
        }

        public FeatureAttribute(Type subjectType, string subject) : base(subjectType, subject)
        {
        }

        public FeatureAttribute(string subject) : base(subject)
        {
        }
    }

    public class StoryAttribute : SubjectAttribute
    {
        public StoryAttribute(Type subjectType) : base(subjectType)
        {
        }

        public StoryAttribute(Type subjectType, string subject) : base(subjectType, subject)
        {
        }

        public StoryAttribute(string subject) : base(subject)
        {
        }
    }
}
